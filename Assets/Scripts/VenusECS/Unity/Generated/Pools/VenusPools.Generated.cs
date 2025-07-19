//Generated automatically, dont touch with hands!
using System;
using System.Collections.Generic;
using VenusECS.Core.Pool;
using VenusECS.Core;
using VenusECS.Unity.Components;


namespace VenusECS.Unity
{
    public partial class VenusPools
    {
        public readonly VenusPool<ActorReference> ActorReferencePool = new VenusPool<ActorReference>();
        public readonly VenusPool<EmptyComponent> EmptyComponentPool = new VenusPool<EmptyComponent>();
        public readonly VenusPool<OtherTestComponent> OtherTestComponentPool = new VenusPool<OtherTestComponent>();
        public readonly VenusPool<PositionComponent> PositionComponentPool = new VenusPool<PositionComponent>();
        public readonly VenusPool<SpawnedEntityComponent> SpawnedEntityComponentPool = new VenusPool<SpawnedEntityComponent>();
        public readonly VenusPool<TagComponent> TagComponentPool = new VenusPool<TagComponent>();
        public readonly VenusPool<TestComponent> TestComponentPool = new VenusPool<TestComponent>();
        public readonly VenusPool<VenusEntityComponent> VenusEntityComponentPool = new VenusPool<VenusEntityComponent>();


        private void AddGenerated()
        {
            _pools.Add(typeof(ActorReference), ActorReferencePool);
            _pools.Add(typeof(EmptyComponent), EmptyComponentPool);
            _pools.Add(typeof(OtherTestComponent), OtherTestComponentPool);
            _pools.Add(typeof(PositionComponent), PositionComponentPool);
            _pools.Add(typeof(SpawnedEntityComponent), SpawnedEntityComponentPool);
            _pools.Add(typeof(TagComponent), TagComponentPool);
            _pools.Add(typeof(TestComponent), TestComponentPool);
            _pools.Add(typeof(VenusEntityComponent), VenusEntityComponentPool);

        }
    }
}
