//Generated automatically, dont touch with hands!
using System;
using System.Collections.Generic;
using VenusECS.Core.Pool;
using VenusECS.Core;
using VenusECS.Unity.Components;
using VenusECS.Unity.Components.Network;
using VenusECS.Unity.Components.Movement;


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
        public readonly VenusPool<OwnershipComponent> OwnershipComponentPool = new VenusPool<OwnershipComponent>();
        public readonly VenusPool<OwnershipRequestComponent> OwnershipRequestComponentPool = new VenusPool<OwnershipRequestComponent>();
        public readonly VenusPool<InputRecieverComponent> InputRecieverComponentPool = new VenusPool<InputRecieverComponent>();
        public readonly VenusPool<LocalInputRecieverComponent> LocalInputRecieverComponentPool = new VenusPool<LocalInputRecieverComponent>();


        private void AddGenerated()
        {
            _pools.Add(typeof(ActorReference), ActorReferencePool);
            ActorReferencePool.PoolIndex = 0;
            _pools.Add(typeof(EmptyComponent), EmptyComponentPool);
            EmptyComponentPool.PoolIndex = 1;
            _pools.Add(typeof(OtherTestComponent), OtherTestComponentPool);
            OtherTestComponentPool.PoolIndex = 2;
            _pools.Add(typeof(PositionComponent), PositionComponentPool);
            PositionComponentPool.PoolIndex = 3;
            _pools.Add(typeof(SpawnedEntityComponent), SpawnedEntityComponentPool);
            SpawnedEntityComponentPool.PoolIndex = 4;
            _pools.Add(typeof(TagComponent), TagComponentPool);
            TagComponentPool.PoolIndex = 5;
            _pools.Add(typeof(TestComponent), TestComponentPool);
            TestComponentPool.PoolIndex = 6;
            _pools.Add(typeof(VenusEntityComponent), VenusEntityComponentPool);
            VenusEntityComponentPool.PoolIndex = 7;
            _pools.Add(typeof(OwnershipComponent), OwnershipComponentPool);
            OwnershipComponentPool.PoolIndex = 8;
            _pools.Add(typeof(OwnershipRequestComponent), OwnershipRequestComponentPool);
            OwnershipRequestComponentPool.PoolIndex = 9;
            _pools.Add(typeof(InputRecieverComponent), InputRecieverComponentPool);
            InputRecieverComponentPool.PoolIndex = 10;
            _pools.Add(typeof(LocalInputRecieverComponent), LocalInputRecieverComponentPool);
            LocalInputRecieverComponentPool.PoolIndex = 11;

        }
    }
}
