using NUnit.Framework;
using UnityEngine;
using VenusECS.Core;
using VenusECS.Unity.Components;
using VenusECS.Unity.Extensions;

public partial class SimplePoolTests
{
    [Test]
    public void SimplePoolTestPass()
    {
        Venus.Reset();
        var entitiesList = new System.Collections.Generic.List<VenusEntity>();
        for (int i = 0; i < 2; i++)
        {
            var entity = Venus.Pools.CreateEntity();
            var testComponent = entity.Add<TestComponent>();
            testComponent.SomeData = 100 + i;
            entitiesList.Add(entity);
        }

        for (int i = 0; i < 2; i++)
        {
            var testComponent = entitiesList[i].Get<TestComponent>();
            Assert.That(testComponent.SomeData == 100 + i);
        }
    }
    
    [Test]
    public void SimplePoolTestsMediumPass()
    {
        Venus.Reset();
        var entitiesList = new System.Collections.Generic.List<VenusEntity>();
        for (int i = 0; i < 20; i++)
        {
            var entity = Venus.Pools.CreateEntity();
            var testComponent = entity.Add<TestComponent>();
            testComponent.SomeData = 100 + i;
            var otherTestComponent = entity.Add<OtherTestComponent>();
            otherTestComponent.OtherData = 1000 + i;
            entitiesList.Add(entity);
        }

        for (int i = 0; i < 20; i++)
        {
            var testComponent = entitiesList[i].Get<TestComponent>();
            Assert.That(testComponent.SomeData == 100 + i);
            var otherComponent = entitiesList[i].Get<OtherTestComponent>();
            Assert.That(otherComponent.OtherData == 1000 + i);
        }
    }

    [Test]
    public void SimplePoolBigEntitiesNumberPass()
    {
        Venus.Reset();
        var entitiesList = new System.Collections.Generic.List<VenusEntity>();
        for (int i = 0; i < 20000; i++)
        {
            var entity = Venus.Pools.CreateEntity();
            var testComponent = entity.Add<TestComponent>();
            testComponent.SomeData = 100000 + i;
            var otherTestComponent = entity.Add<OtherTestComponent>();
            otherTestComponent.OtherData = 1000000 + i;
            entitiesList.Add(entity);
        }

        for (int i = 0; i < 20000; i++)
        {
            var testComponent = entitiesList[i].Get<TestComponent>();
            Assert.That(testComponent.SomeData == 100000 + i);
            var otherComponent = entitiesList[i].Get<OtherTestComponent>();
            Assert.That(otherComponent.OtherData == 1000000 + i);
        }
    }

    [Test]
    public void SimplePoolEntitiesRemovingPass()
    {
        Venus.Reset();
        var entitiesList = new System.Collections.Generic.List<VenusEntity>();
        for (int i = 0; i < 2; i++)
        {
            var entity = Venus.Pools.CreateEntity();
            var testComponent = entity.Add<TestComponent>();
            testComponent.SomeData = 100 + i;
            entitiesList.Add(entity);
        }

        for (int i = 0; i < 2; i++)
        {
            Venus.Pools.DeleteEntity(entitiesList[i]);
        }
        
        for (int i = 0; i < 2; i++)
        {
            var entity = Venus.Pools.CreateEntity();
            var testComponent = entity.Add<TestComponent>();
            testComponent.SomeData = 100 + i;
            entitiesList.Add(entity);
        }
        
        for (int i = 0; i < 2; i++)
        {
            var testComponent = entitiesList[i].Get<TestComponent>();
            Assert.That(testComponent.SomeData == 100 + i);
        }
    }

    [Test]
    public void SimplePoolComponentsRemovingPass()
    {
        Venus.Reset();
        var entitiesList = new System.Collections.Generic.List<VenusEntity>();
        for (int i = 0; i < 2; i++)
        {
            var entity = Venus.Pools.CreateEntity();
            var testComponent = entity.Add<TestComponent>();
            testComponent.SomeData = 100 + i;
            entitiesList.Add(entity);
        }

        for (int i = 0; i < 2; i++)
        {
            entitiesList[i].Remove<TestComponent>();
        }
        
        for (int i = 0; i < 2; i++)
        {
            Assert.That(!entitiesList[i].Has<TestComponent>());
            var testComponent = entitiesList[i].Add<TestComponent>();
            testComponent.SomeData = 10000 + i;
        }
        
        for (int i = 0; i < 2; i++)
        {
            var testComponent = entitiesList[i].Get<TestComponent>();
            Assert.That(testComponent.SomeData == 10000 + i);
        }
    }
}