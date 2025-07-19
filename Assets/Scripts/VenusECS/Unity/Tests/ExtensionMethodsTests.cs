using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity.Components;
using VenusECS.Unity.Extensions;

public partial class SimplePoolTests
{
    public class ExtensionMethodsTests
    {
        [SetUp]
        public void Setup()
        {
            Venus.Reset();
        }
        
        // This test simulates what the generated extension methods would do
        [Test]
        public void DirectPoolAccess_PerformsSameAsGenericMethods()
        {
            // Create test entity
            var entity = Venus.Pools.CreateEntity();
            
            // Get pools directly (this is what the generated extension methods would do)
            var testPool = Venus.Pools.GetPool<TestComponent>();
            var otherTestPool = Venus.Pools.GetPool<OtherTestComponent>();
            
            // ---- Test Add ----
            
            // Direct typed access
            ref var testComponent = ref ((VenusPool<TestComponent>)testPool).AddTyped(entity);
            testComponent.SomeData = 42;
            
            // Check component exists
            Assert.IsTrue(testPool.HasTyped(entity));
            Assert.AreEqual(42, ((VenusPool<TestComponent>)testPool).GetTyped(entity).SomeData);
            
            // ---- Test Get ----
            
            // Direct typed access
            ref var getComponent = ref ((VenusPool<TestComponent>)testPool).GetTyped(entity);
            getComponent.SomeData = 100;
            
            // Check value updated
            Assert.AreEqual(100, ((VenusPool<TestComponent>)testPool).GetTyped(entity).SomeData);
            
            // ---- Test Has ----
            
            bool hasComponent = testPool.HasTyped(entity);
            bool hasOtherComponent = otherTestPool.HasTyped(entity);
            
            Assert.IsTrue(hasComponent);
            Assert.IsFalse(hasOtherComponent);
            
            // ---- Test Del ----
            
            ((VenusPool<TestComponent>)testPool).RemoveTyped(entity);
            
            // Check component removed
            Assert.IsFalse(testPool.HasTyped(entity));
            
            // ---- Test DelIfExists ----
            
            // Add component again
            ((VenusPool<TestComponent>)testPool).AddTyped(entity);
            
            // Delete if exists
            if (testPool.HasTyped(entity))
            {
                ((VenusPool<TestComponent>)testPool).RemoveTyped(entity);
            }
            
            // Check component removed
            Assert.IsFalse(testPool.HasTyped(entity));
            
            // Try delete if exists when doesn't exist (shouldn't throw)
            if (testPool.HasTyped(entity))
            {
                ((VenusPool<TestComponent>)testPool).RemoveTyped(entity);
            }
            
            // ---- Test GetOrAdd ----
            
            ref var orAddComponent = ref (testPool.HasTyped(entity) ? 
                ref ((VenusPool<TestComponent>)testPool).GetTyped(entity) : 
                ref ((VenusPool<TestComponent>)testPool).AddTyped(entity));
            
            orAddComponent.SomeData = 200;
            
            // Verify component exists and has correct value
            Assert.IsTrue(testPool.HasTyped(entity));
            Assert.AreEqual(200, ((VenusPool<TestComponent>)testPool).GetTyped(entity).SomeData);
        }
        
        [Test]
        public void DirectPoolMethods_ShouldUseUnsafeTypeConversion()
        {
            // Create test entity
            var entity = Venus.Pools.CreateEntity();
            
            // Get test pool both generically and directly
            var testPool = Venus.Pools.GetPool<TestComponent>();
            
            // Add component using generic method
            ref var component1 = ref entity.Add<TestComponent>();
            component1.SomeData = 100;
            
            // Add component using direct typed method on a different entity
            var entity2 = Venus.Pools.CreateEntity();
            ref var component2 = ref ((VenusPool<TestComponent>)testPool).AddTyped(entity2);
            component2.SomeData = 200;
            
            // Verify both components were added correctly
            Assert.AreEqual(100, entity.Get<TestComponent>().SomeData);
            Assert.AreEqual(200, ((VenusPool<TestComponent>)testPool).GetTyped(entity2).SomeData);
        }
        
        [Test]
        public void MultipleComponentTypes_DirectAccess()
        {
            // Create test entity
            var entity = Venus.Pools.CreateEntity();
            
            // Get pools directly
            var testPool = Venus.Pools.GetPool<TestComponent>();
            var otherPool = Venus.Pools.GetPool<OtherTestComponent>();
            var posPool = Venus.Pools.GetPool<PositionComponent>();
            
            // Add different component types
            ref var test = ref ((VenusPool<TestComponent>)testPool).AddTyped(entity);
            test.SomeData = 42;
            
            ref var other = ref ((VenusPool<OtherTestComponent>)otherPool).AddTyped(entity);
            other.OtherData = 100;
            
            ref var pos = ref ((VenusPool<PositionComponent>)posPool).AddTyped(entity);
            pos.Position = new Vector2(1, 2);
            
            // Verify all components exist and have correct values
            Assert.IsTrue(testPool.HasTyped(entity));
            Assert.IsTrue(otherPool.HasTyped(entity));
            Assert.IsTrue(posPool.HasTyped(entity));
            
            Assert.AreEqual(42, ((VenusPool<TestComponent>)testPool).GetTyped(entity).SomeData);
            Assert.AreEqual(100, ((VenusPool<OtherTestComponent>)otherPool).GetTyped(entity).OtherData);
            Assert.AreEqual(new Vector2(1, 2), ((VenusPool<PositionComponent>)posPool).GetTyped(entity).Position);
        }
        
        [Test]
        public void SetComponent_DirectAccess()
        {
            // Create test entity
            var entity = Venus.Pools.CreateEntity();
            
            // Get test pool
            var testPool = Venus.Pools.GetPool<TestComponent>();
            
            // Create component to set
            var newComponent = new TestComponent { SomeData = 42 };
            
            // Set component when it doesn't exist yet (should add)
            if (testPool.HasTyped(entity))
            {
                ((VenusPool<TestComponent>)testPool).GetTyped(entity) = newComponent;
            }
            else
            {
                ((VenusPool<TestComponent>)testPool).AddTyped(entity) = newComponent;
            }
            
            // Verify component was added with correct values
            Assert.IsTrue(testPool.HasTyped(entity));
            Assert.AreEqual(42, ((VenusPool<TestComponent>)testPool).GetTyped(entity).SomeData);
            
            // Create a new component to set
            var updatedComponent = new TestComponent { SomeData = 100 };
            
            // Set component when it already exists (should update)
            if (testPool.HasTyped(entity))
            {
                ((VenusPool<TestComponent>)testPool).GetTyped(entity) = updatedComponent;
            }
            else
            {
                ((VenusPool<TestComponent>)testPool).AddTyped(entity) = updatedComponent;
            }
            
            // Verify component was updated with new values
            Assert.IsTrue(testPool.HasTyped(entity));
            Assert.AreEqual(100, ((VenusPool<TestComponent>)testPool).GetTyped(entity).SomeData);
        }
        
        [Test]
        public void DirectPoolAccess_WithFilter()
        {
            // Create entities
            for (int i = 0; i < 10; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                
                // Get pools directly
                var testPool = Venus.Pools.GetPool<TestComponent>();
                
                // Add TestComponent to all entities
                ref var test = ref ((VenusPool<TestComponent>)testPool).AddTyped(entity);
                test.SomeData = i;
                
                // Add OtherTestComponent to even entities
                if (i % 2 == 0)
                {
                    var otherPool = Venus.Pools.GetPool<OtherTestComponent>();
                    ref var other = ref ((VenusPool<OtherTestComponent>)otherPool).AddTyped(entity);
                    other.OtherData = i * 10;
                }
            }
            
            // Create filter for TestComponent
            var filter = new VenusFilter(new Include<TestComponent>());
            
            // Iterate through filter
            int count = 0;
            foreach (var entity in filter)
            {
                count++;
                
                // Access component using direct pool access
                var testPool = Venus.Pools.GetPool<TestComponent>();
                var testComp = ((VenusPool<TestComponent>)testPool).GetTyped(entity);
                
                Assert.IsTrue(testComp.SomeData >= 0 && testComp.SomeData < 10);
            }
            
            Assert.AreEqual(10, count, "Filter should find all 10 entities");
            
            // Create filter for entities with both components
            var bothFilter = new VenusFilter(new Include<TestComponent, OtherTestComponent>());
            
            count = 0;
            foreach (var entity in bothFilter)
            {
                count++;
                
                // Direct access to components
                var testPool = Venus.Pools.GetPool<TestComponent>();
                var otherPool = Venus.Pools.GetPool<OtherTestComponent>();
                
                var testComp = ((VenusPool<TestComponent>)testPool).GetTyped(entity);
                var otherComp = ((VenusPool<OtherTestComponent>)otherPool).GetTyped(entity);
                
                // Verify entity has expected components
                Assert.IsTrue(testPool.HasTyped(entity));
                Assert.IsTrue(otherPool.HasTyped(entity));
                
                // Verify component values match expected pattern
                Assert.AreEqual(testComp.SomeData * 10, otherComp.OtherData);
                Assert.IsTrue(testComp.SomeData % 2 == 0, "Entity should have even ID");
            }
            
            Assert.AreEqual(5, count, "Filter should find 5 entities with both components");
        }
    }
} 