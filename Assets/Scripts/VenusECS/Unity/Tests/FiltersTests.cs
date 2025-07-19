using NUnit.Framework;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity.Components;
using VenusECS.Unity.Extensions;

public partial class SimplePoolTests
{
    public class FiltersTests
    {
        [Test]
        public void IncludeFilterTest()
        {
            Venus.Reset();

            var entitiesList = new System.Collections.Generic.List<VenusEntity>();
            for (int i = 0; i < 20; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                entitiesList.Add(entity);
                ref var testComponent = ref entity.Add<TestComponent>();
                testComponent.SomeData = 300 + entity.Id;

                if (i <= 10)
                {
                    ref var otherTestComponent = ref entity.Add<OtherTestComponent>();
                    otherTestComponent.OtherData = 1200 + entity.Id;
                }
            }

            var filter = new VenusFilter(new Include<TestComponent, OtherTestComponent>());

            foreach(var entity in filter)
            {
                var testComponent = entity.Get<TestComponent>();
                var otherTestComponent = entity.Get<OtherTestComponent>();
                Assert.That(testComponent.SomeData == 300 + entity.Id);
                Assert.That(otherTestComponent.OtherData == 1200 + entity.Id);
            }
        }

        [Test]
        public void ExcludeFilterTest()
        {
            Venus.Reset();

            var entitiesList = new System.Collections.Generic.List<VenusEntity>();
            for (int i = 0; i < 20; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                entitiesList.Add(entity);
                ref var testComponent = ref entity.Add<TestComponent>();
                testComponent.SomeData = 300 + entity.Id;

                if (i <= 10)
                {
                    ref var otherTestComponent = ref entity.Add<OtherTestComponent>();
                    otherTestComponent.OtherData = 1200 + entity.Id;
                }
            }

            var filter = new VenusFilter(new Include<OtherTestComponent>(), new Exclude<TestComponent>());

            foreach (var entity in filter)
            {
                var otherTestComponent = entity.Get<OtherTestComponent>();
                Assert.That(!entity.Has<TestComponent>());
                Assert.That(otherTestComponent.OtherData == 1200 + entity.Id);
            }
        }

        [Test]
        public void MultipleIterationsFilterTest()
        {
            Venus.Reset();

            var entitiesList = new System.Collections.Generic.List<VenusEntity>();
            for (int i = 0; i < 20; i++)
            {
                var entity = Venus.Pools.CreateEntity();
                entitiesList.Add(entity);
                ref var testComponent = ref entity.Add<TestComponent>();
                testComponent.SomeData = 300 + entity.Id;

                if (i <= 10)
                {
                    ref var otherTestComponent = ref entity.Add<OtherTestComponent>();
                    otherTestComponent.OtherData = 1200 + entity.Id;
                }
            }

            var filter = new VenusFilter(new Include<OtherTestComponent>(), new Exclude<TestComponent>());

            foreach (var entity in filter)
            {
                var otherTestComponent = entity.Get<OtherTestComponent>();
                Assert.That(!entity.Has<TestComponent>());
                Assert.That(otherTestComponent.OtherData == 1200 + entity.Id);
            }

            foreach (var entity in filter)
            {
                var otherTestComponent = entity.Get<OtherTestComponent>();
                Assert.That(!entity.Has<TestComponent>());
                Assert.That(otherTestComponent.OtherData == 1200 + entity.Id);
            }
        }
    }
}