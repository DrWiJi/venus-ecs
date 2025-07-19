using System.Collections.Generic;
using NUnit.Framework;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity.Components;
using VenusECS.Unity.Extensions;

public partial class SimplePoolTests
{
    public class LargeEntitySetTests
    {
        private const int LARGE_ENTITY_COUNT = 1440;
        private const int LARGE_MULTIPLE_ENTITY_COUNT = 2*3*5*6*7;
        
        [SetUp]
        public void Setup()
        {
            Venus.Reset();
        }

        [TearDown]
        public void TearDown()
        {
            Venus.Reset();
        }

        [Test]
        public void LargeEntitySet_BasicFiltering()
        {
            // Create a large number of entities with various component patterns
            for (int i = 0; i < LARGE_ENTITY_COUNT; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                
                // Add TagComponent to all entities
                ref var tag = ref entity.Add<TagComponent>(); 
                tag.Tag = i;
                
                // Add TestComponent to even entities
                if (i % 2 == 0)
                {
                    ref var test = ref entity.Add<TestComponent>();
                    test.SomeData = i;
                }
                
                // Add PositionComponent to entities divisible by 3
                if (i % 3 == 0)
                {
                    ref var pos = ref entity.Add<PositionComponent>();
                    pos.Position = new UnityEngine.Vector2(i, i);
                }
                
                // Add OtherTestComponent to entities divisible by 5
                if (i % 5 == 0)
                {
                    ref var other = ref entity.Add<OtherTestComponent>();
                    other.OtherData = i;
                }
            }
            
            // Test filter with one component
            var testFilter = new VenusFilter(new Include<TestComponent>());
            int testCount = 0;
            foreach (var entity in testFilter)
            {
                testCount++;
                Assert.IsTrue(entity.Has<TestComponent>());
                Assert.IsTrue(entity.Get<TagComponent>().Tag % 2 == 0);
            }
            Assert.AreEqual(LARGE_ENTITY_COUNT / 2, testCount);
            
            // Test filter with multiple components (AND logic)
            var multiFilter = new VenusFilter(new Include<TestComponent, PositionComponent>());
            int multiCount = 0;
            foreach (var entity in multiFilter)
            {
                multiCount++;
                Assert.IsTrue(entity.Has<TestComponent>());
                Assert.IsTrue(entity.Has<PositionComponent>());
                
                var tag = entity.Get<TagComponent>().Tag;
                Assert.IsTrue(tag % 2 == 0, "Entity ID should be even");
                Assert.IsTrue(tag % 3 == 0, "Entity ID should be divisible by 3");
            }
            // Expected count: entities divisible by both 2 and 3 = divisible by 6
            Assert.AreEqual(LARGE_ENTITY_COUNT / 6, multiCount);
            
            // Test filter with Exclude
            var excludeFilter = new VenusFilter(
                new Include<TestComponent>(),
                new Exclude<OtherTestComponent>()
            );
            
            int excludeCount = 0;
            foreach (var entity in excludeFilter)
            {
                excludeCount++;
                Assert.IsTrue(entity.Has<TestComponent>());
                Assert.IsFalse(entity.Has<OtherTestComponent>());
                
                var tag = entity.Get<TagComponent>().Tag;
                Assert.IsTrue(tag % 2 == 0, "Entity ID should be even");
                Assert.IsFalse(tag % 5 == 0, "Entity ID should not be divisible by 5");
            }
            
            // Expected count: even numbers that are not divisible by 5
            // Out of every 10 numbers: 0,2,4,6,8 are even, and 0,5 are divisible by 5
            // So in every 10 numbers, 4 match (2,4,6,8)
            Assert.AreEqual(LARGE_ENTITY_COUNT * 4 / 10, excludeCount);
        }
        
        [Test]
        public void LargeEntitySet_DynamicComponentChanges()
        {
            // Create entities
            var entities = new List<VenusEntity>();
            for (int i = 0; i < LARGE_ENTITY_COUNT; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                entities.Add(entity);
                
                // Start with all entities having TagComponent only
                ref var tag = ref entity.Add<TagComponent>();
                tag.Tag = i;
            }
            
            // Create filter for TestComponent
            var filter = new VenusFilter(new Include<TestComponent>());
            
            // Initially no entities have TestComponent
            int initialCount = 0;
            foreach (var entity in filter)
            {
                initialCount++;
            }
            Assert.AreEqual(0, initialCount);
            
            // Add TestComponent to all odd-indexed entities
            for (int i = 0; i < entities.Count; i++)
            {
                if (i % 2 == 1)
                {
                    ref var test = ref entities[i].Add<TestComponent>();
                    test.SomeData = i;
                }
            }
            
            // Count should now be half of entities
            int midCount = 0;
            foreach (var entity in filter)
            {
                midCount++;
                Assert.IsTrue(entity.Has<TestComponent>());
                Assert.IsTrue(entity.Get<TagComponent>().Tag % 2 == 1);
            }
            Assert.AreEqual(LARGE_ENTITY_COUNT / 2, midCount);
            
            // Add TestComponent to all remaining entities
            for (int i = 0; i < entities.Count; i++)
            {
                if (i % 2 == 0)
                {
                    ref var test = ref entities[i].GetOrAdd<TestComponent>();
                    test.SomeData = i;
                }
            }
            
            // Now all entities should match
            int finalCount = 0;
            foreach (var entity in filter)
            {
                finalCount++;
            }
            Assert.AreEqual(LARGE_ENTITY_COUNT/2, finalCount);
            
            // Remove TestComponent from all entities
            foreach (var entity in entities)
            {
                entity.DelIfExists<TestComponent>();
            }
            
            // Count should be back to zero
            int afterRemoveCount = 0;
            foreach (var entity in filter)
            {
                afterRemoveCount++;
            }
            Assert.AreEqual(0, afterRemoveCount);
        }
        
        [Test]
        public void LargeEntitySet_SparseEntities()
        {
            // Create entities with very large IDs (sparse entity set)
            const int STEP = 1000; // Large gap between entity IDs
            
            var entities = new List<VenusEntity>();
            for (int i = 0; i < 100; i++) // Create 100 sparse entities
            {
                var entity = Venus.Pools.CreateEntity();
                entities.Add(entity);
                
                // Set a very large ID (emulating sparse storage)
                // Note: in a real scenario, we wouldn't manually set IDs, but this is for testing
                entity.Id = i * STEP;
                
                entity.Add<TestComponent>();
            }
            
            // Test filter
            var filter = new VenusFilter(new Include<TestComponent>());
            
            int count = 0;
            foreach (var entity in filter)
            {
                count++;
                Assert.IsTrue(entity.Has<TestComponent>());
            }
            
            Assert.AreEqual(100, count, "Filter should match all 100 sparse entities");
        }
        
        [Test]
        public void LargeEntitySet_MultipleFilters()
        {
            // Create large set of entities with different component combinations
            for (int i = 0; i < LARGE_MULTIPLE_ENTITY_COUNT; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                
                if (i % 2 == 0) entity.Add<TestComponent>();
                if (i % 3 == 0) entity.Add<OtherTestComponent>();
                if (i % 5 == 0) entity.Add<PositionComponent>();
                if (i % 7 == 0) entity.Add<EmptyComponent>();
            }
            
            // Create several different filters
            var filter1 = new VenusFilter(new Include<TestComponent>());
            var filter2 = new VenusFilter(new Include<OtherTestComponent>());
            var filter3 = new VenusFilter(new Include<PositionComponent>());
            var filter4 = new VenusFilter(new Include<EmptyComponent>());
            
            // Combined filters
            var filter5 = new VenusFilter(
                new Include<TestComponent, OtherTestComponent>()
            );
            
            var filter6 = new VenusFilter(
                new Include<TestComponent>(),
                new Exclude<OtherTestComponent>()
            );
            
            // Count matches for each filter
            int count1 = 0, count2 = 0, count3 = 0, count4 = 0, count5 = 0, count6 = 0;
            
            foreach (var entity in filter1) count1++;
            foreach (var entity in filter2) count2++;
            foreach (var entity in filter3) count3++;
            foreach (var entity in filter4) count4++;
            foreach (var entity in filter5) count5++;
            foreach (var entity in filter6) count6++;
            
            // Check counts match expected values
            Assert.AreEqual(LARGE_MULTIPLE_ENTITY_COUNT / 2, count1, "Filter1 count incorrect");
            Assert.AreEqual(LARGE_MULTIPLE_ENTITY_COUNT / 3, count2, "Filter2 count incorrect");
            Assert.AreEqual(LARGE_MULTIPLE_ENTITY_COUNT / 5, count3, "Filter3 count incorrect");
            Assert.AreEqual(LARGE_MULTIPLE_ENTITY_COUNT / 7, count4, "Filter4 count incorrect");
            
            // Filter5: entities divisible by both 2 and 3 = divisible by 6
            Assert.AreEqual(LARGE_MULTIPLE_ENTITY_COUNT / 6, count5, "Filter5 count incorrect");
            
            // Filter6: entities divisible by 2 but not by 3
            // Out of every 6 numbers, 3 are even and 2 are divisible by 3
            // Numbers divisible by both 2 and 3 = 1 (e.g., 0,6,12)
            // So entities divisible by 2 but not 3 = 2 for every 6
            Assert.AreEqual(LARGE_MULTIPLE_ENTITY_COUNT / 3, count6, "Filter6 count incorrect");
        }
    }
} 