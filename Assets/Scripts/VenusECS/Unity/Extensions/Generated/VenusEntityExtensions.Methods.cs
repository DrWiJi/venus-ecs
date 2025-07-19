//Generated automatically, dont touch with hands!
using System;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity;
using VenusECS.Unity.Components;


namespace VenusECS.Extensions
{
    public static class VenusEntityExtensions
    {
        public static ref ActorReference AddActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            return ref pool.AddTyped(entity);
        }

        public static void DelActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static ref ActorReference GetActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            return ref pool.GetTyped(entity);
        }

        public static ref ActorReference GetOrAddActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            if (pool.HasTyped(entity))
            {
                return ref pool.GetTyped(entity);
            }
            return ref pool.AddTyped(entity);
        }

        public static bool HasActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            return pool.HasTyped(entity);
        }

        public static void SetActorReference(this VenusEntity entity, ActorReference component)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            if (pool.HasTyped(entity))
            {
                pool.GetTyped(entity) = component;
            }
            else
            {
                pool.AddTyped(entity) = component;
            }
        }

        public static ref EmptyComponent AddEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            return ref pool.AddTyped(entity);
        }

        public static void DelEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static ref EmptyComponent GetEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            return ref pool.GetTyped(entity);
        }

        public static ref EmptyComponent GetOrAddEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            if (pool.HasTyped(entity))
            {
                return ref pool.GetTyped(entity);
            }
            return ref pool.AddTyped(entity);
        }

        public static bool HasEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            return pool.HasTyped(entity);
        }

        public static void SetEmptyComponent(this VenusEntity entity, EmptyComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.GetTyped(entity) = component;
            }
            else
            {
                pool.AddTyped(entity) = component;
            }
        }

        public static ref OtherTestComponent AddOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            return ref pool.AddTyped(entity);
        }

        public static void DelOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static ref OtherTestComponent GetOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            return ref pool.GetTyped(entity);
        }

        public static ref OtherTestComponent GetOrAddOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            if (pool.HasTyped(entity))
            {
                return ref pool.GetTyped(entity);
            }
            return ref pool.AddTyped(entity);
        }

        public static bool HasOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            return pool.HasTyped(entity);
        }

        public static void SetOtherTestComponent(this VenusEntity entity, OtherTestComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.GetTyped(entity) = component;
            }
            else
            {
                pool.AddTyped(entity) = component;
            }
        }

        public static ref PositionComponent AddPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            return ref pool.AddTyped(entity);
        }

        public static void DelPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static ref PositionComponent GetPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            return ref pool.GetTyped(entity);
        }

        public static ref PositionComponent GetOrAddPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            if (pool.HasTyped(entity))
            {
                return ref pool.GetTyped(entity);
            }
            return ref pool.AddTyped(entity);
        }

        public static bool HasPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            return pool.HasTyped(entity);
        }

        public static void SetPositionComponent(this VenusEntity entity, PositionComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.GetTyped(entity) = component;
            }
            else
            {
                pool.AddTyped(entity) = component;
            }
        }

        public static ref SpawnedEntityComponent AddSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            return ref pool.AddTyped(entity);
        }

        public static void DelSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static ref SpawnedEntityComponent GetSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            return ref pool.GetTyped(entity);
        }

        public static ref SpawnedEntityComponent GetOrAddSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            if (pool.HasTyped(entity))
            {
                return ref pool.GetTyped(entity);
            }
            return ref pool.AddTyped(entity);
        }

        public static bool HasSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            return pool.HasTyped(entity);
        }

        public static void SetSpawnedEntityComponent(this VenusEntity entity, SpawnedEntityComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.GetTyped(entity) = component;
            }
            else
            {
                pool.AddTyped(entity) = component;
            }
        }

        public static ref TagComponent AddTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            return ref pool.AddTyped(entity);
        }

        public static void DelTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static ref TagComponent GetTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            return ref pool.GetTyped(entity);
        }

        public static ref TagComponent GetOrAddTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            if (pool.HasTyped(entity))
            {
                return ref pool.GetTyped(entity);
            }
            return ref pool.AddTyped(entity);
        }

        public static bool HasTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            return pool.HasTyped(entity);
        }

        public static void SetTagComponent(this VenusEntity entity, TagComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.GetTyped(entity) = component;
            }
            else
            {
                pool.AddTyped(entity) = component;
            }
        }

        public static ref TestComponent AddTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            return ref pool.AddTyped(entity);
        }

        public static void DelTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static ref TestComponent GetTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            return ref pool.GetTyped(entity);
        }

        public static ref TestComponent GetOrAddTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            if (pool.HasTyped(entity))
            {
                return ref pool.GetTyped(entity);
            }
            return ref pool.AddTyped(entity);
        }

        public static bool HasTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            return pool.HasTyped(entity);
        }

        public static void SetTestComponent(this VenusEntity entity, TestComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.GetTyped(entity) = component;
            }
            else
            {
                pool.AddTyped(entity) = component;
            }
        }

        public static ref VenusEntityComponent AddVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            return ref pool.AddTyped(entity);
        }

        public static void DelVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static ref VenusEntityComponent GetVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            return ref pool.GetTyped(entity);
        }

        public static ref VenusEntityComponent GetOrAddVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            if (pool.HasTyped(entity))
            {
                return ref pool.GetTyped(entity);
            }
            return ref pool.AddTyped(entity);
        }

        public static bool HasVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            return pool.HasTyped(entity);
        }

        public static void SetVenusEntityComponent(this VenusEntity entity, VenusEntityComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.GetTyped(entity) = component;
            }
            else
            {
                pool.AddTyped(entity) = component;
            }
        }


    }
}
