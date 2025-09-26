using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity.Components;
using VenusECS.Unity.Extensions;

public partial class SimplePoolTests
{
    public class FilterCornerCaseTests
    {
        [SetUp]
        public void Setup()
        {
            Venus.Reset();
        }

        [Test]
        public void EmptyFilter_NoEntitiesFound()
        {
            // Create entities with components
            for (int i = 0; i < 10; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                entity.Add<TestComponent>();
            }

            // Create filter for a component that hasn't been added to any entity
            var filter = new VenusFilter(new Include<EmptyComponent>());
            
            int count = 0;
            foreach (var entity in filter)
            {
                count++;
            }
            
            Assert.AreEqual(0, count, "Empty filter should not return any entities");
        }

        [Test]
        public void EmptyEntityPool_NoEntitiesFound()
        {
            // Create a filter without creating any entities
            var filter = new VenusFilter(new Include<TestComponent>());
            
            int count = 0;
            foreach (var entity in filter)
            {
                count++;
            }
            
            Assert.AreEqual(0, count, "Filter with no entities should return empty result");
        }
        
        [Test]
        public void DynamicComponentAddition_AffectsIteration()
        {
            // Create entities
            var entities = new List<VenusEntity>();
            for (int i = 0; i < 10; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                entities.Add(entity);
                
                // Only add TestComponent to half the entities initially
                if (i % 2 == 0)
                {
                    entity.Add<TestComponent>();
                }
            }
            
            // Use a filter for TestComponent
            var filter = new VenusFilter(new Include<TestComponent>());
            
            int initialCount = 0;
            foreach (var entity in filter)
            {
                initialCount++;
            }
            Assert.AreEqual(5, initialCount, "Should initially find 5 entities");
            
            // Add TestComponent to all remaining entities
            foreach (var entity in entities)
            {
                if (!entity.Has<TestComponent>())
                {
                    entity.Add<TestComponent>();
                }
            }
            
            // The filter should now return all entities
            int newCount = 0;
            foreach (var entity in filter)
            {
                newCount++;
            }
            Assert.AreEqual(10, newCount, "Should find 10 entities after adding components");
        }
        
        [Test]
        public void DynamicComponentRemoval_AffectsIteration()
        {
            // Create entities
            var entities = new List<VenusEntity>();
            for (int i = 0; i < 10; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                entities.Add(entity);
                entity.Add<TestComponent>();
            }
            
            // Use a filter for TestComponent
            var filter = new VenusFilter(new Include<TestComponent>());
            
            int initialCount = 0;
            foreach (var entity in filter)
            {
                initialCount++;
            }
            Assert.AreEqual(10, initialCount, "Should initially find 10 entities");
            
            // Remove TestComponent from half the entities
            for (int i = 0; i < 5; i++)
            {
                entities[i].Remove<TestComponent>();
            }
            
            // The filter should now return only half the entities
            int newCount = 0;
            foreach (var entity in filter)
            {
                newCount++;
            }
            Assert.AreEqual(5, newCount, "Should find 5 entities after removing components");
        }
        
        [Test]
        public void ComplexFilter_WithMultipleComponents()
        {
            // Create entities with various component combinations
            for (int i = 0; i < 20; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                
                // All entities have tag
                var tag = entity.Add<TagComponent>();
                tag.Tag = i;
                
                // Add position to even IDs
                if (i % 2 == 0)
                {
                    var pos = entity.Add<PositionComponent>();
                    pos.Position = new Vector2(i, i);
                }
                
                // Add TestComponent to IDs divisible by 3
                if (i % 3 == 0)
                {
                    var test = entity.Add<TestComponent>();
                    test.SomeData = i * 100;
                }
                
                // Add OtherTestComponent to IDs divisible by 4
                if (i % 4 == 0)
                {
                    var other = entity.Add<OtherTestComponent>();
                    other.OtherData = i * 200;
                }
            }
            
            // Filter for entities with Position AND Test BUT NOT OtherTest
            var filter = new VenusFilter(
                new Include<PositionComponent, TestComponent>(),
                new Exclude<OtherTestComponent>()
            );
            
            var matchingEntities = new List<VenusEntity>();
            foreach (var entity in filter)
            {
                matchingEntities.Add(entity);
                
                // Validation
                Assert.IsTrue(entity.Has<PositionComponent>(), "Entity should have PositionComponent");
                Assert.IsTrue(entity.Has<TestComponent>(), "Entity should have TestComponent");
                Assert.IsFalse(entity.Has<OtherTestComponent>(), "Entity should not have OtherTestComponent");
                
                // Calculate expected Tag values - should be divisible by both 2 and 3, but not by 4
                var tag = entity.Get<TagComponent>();
                Assert.IsTrue(tag.Tag % 2 == 0, "Tag should be divisible by 2");
                Assert.IsTrue(tag.Tag % 3 == 0, "Tag should be divisible by 3");
                Assert.IsTrue(tag.Tag % 4 != 0, "Tag should not be divisible by 4");
            }
            
            // Expected matches: entities with IDs 6 and 18 (divisible by 2 and 3 but not 4)
            Assert.AreEqual(2, matchingEntities.Count, "Should match exactly 2 entities");
        }
        
        [Test]
        public void NestedFilters_IndependentIteration()
        {
            // Create entities 
            for (int i = 0; i < 10; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                entity.Add<TestComponent>();
                
                if (i % 2 == 0)
                {
                    entity.Add<OtherTestComponent>();
                }
            }
            
            var testFilter = new VenusFilter(new Include<TestComponent>());
            var otherFilter = new VenusFilter(new Include<OtherTestComponent>());
            
            int outerCount = 0;
            
            // Nested iteration - outer filter should have 10 entities, inner filter 5
            foreach (var outerEntity in testFilter)
            {
                outerCount++;
                
                int innerCount = 0;
                foreach (var innerEntity in otherFilter)
                {
                    innerCount++;
                    
                    // Verify inner entity has both components
                    Assert.IsTrue(innerEntity.Has<TestComponent>());
                    Assert.IsTrue(innerEntity.Has<OtherTestComponent>());
                }
                
                Assert.AreEqual(5, innerCount, "Inner filter should always have 5 entities");
            }
            
            Assert.AreEqual(10, outerCount, "Outer filter should have 10 entities");
        }
        
        [Test]
        public void EmptyComponent_TagFiltering()
        {
            // Create entities with empty component as a tag
            for (int i = 0; i < 10; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                
                if (i % 2 == 0)
                {
                    entity.Add<EmptyComponent>();
                }
            }
            
            // Filter by the empty component
            var filter = new VenusFilter(new Include<EmptyComponent>());
            
            int count = 0;
            foreach (var entity in filter)
            {
                count++;
                Assert.IsTrue(entity.Has<EmptyComponent>());
            }
            
            Assert.AreEqual(5, count, "Should find 5 entities with EmptyComponent");
        }
        
        [Test]
        public void Filter_WithExcludeOnly_ThrowsException()
        {
            // This tests that a filter cannot be created with only an exclude filter
            // It should throw an exception as we need at least one include filter
            
            Assert.Throws<System.NullReferenceException>(() => 
            {
                var filter = new VenusFilter(null, new Exclude<TestComponent>());
            });
        }
    }
} 