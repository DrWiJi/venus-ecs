//Generated automatically, dont touch with hands!
using System;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity;
using VenusECS.Unity.Components;
using VenusECS.Unity.Components.Network;
using VenusECS.Unity.Components.Movement;


namespace VenusECS.Extensions
{
    public static class VenusEntityExtensions
    {
        public static ActorReference AddActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            return pool.AddTyped(entity);
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

        public static ActorReference GetActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            return pool.GetTyped(entity);
        }
        
        public static ActorReference GetSecondaryActorReference(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).ActorReferencePool.GetTyped(entity);
        }

        public static ActorReference GetOrAddActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasActorReference(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryActorReference(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).ActorReferencePool.HasTyped(entity);
        }

        public static void SetActorReference(this VenusEntity entity, ActorReference component)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            pool.SetTyped(entity, component);
        }
        
        public static ActorReference AddActorReference(this VenusEntity entity, ActorReference component)
        {
            var pool = ((VenusPools)Venus.Pools).ActorReferencePool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static EmptyComponent AddEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            return pool.AddTyped(entity);
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

        public static EmptyComponent GetEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static EmptyComponent GetSecondaryEmptyComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).EmptyComponentPool.GetTyped(entity);
        }

        public static EmptyComponent GetOrAddEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasEmptyComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryEmptyComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).EmptyComponentPool.HasTyped(entity);
        }

        public static void SetEmptyComponent(this VenusEntity entity, EmptyComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static EmptyComponent AddEmptyComponent(this VenusEntity entity, EmptyComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).EmptyComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static OtherTestComponent AddOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            return pool.AddTyped(entity);
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

        public static OtherTestComponent GetOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static OtherTestComponent GetSecondaryOtherTestComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).OtherTestComponentPool.GetTyped(entity);
        }

        public static OtherTestComponent GetOrAddOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasOtherTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryOtherTestComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).OtherTestComponentPool.HasTyped(entity);
        }

        public static void SetOtherTestComponent(this VenusEntity entity, OtherTestComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static OtherTestComponent AddOtherTestComponent(this VenusEntity entity, OtherTestComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).OtherTestComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static PositionComponent AddPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            return pool.AddTyped(entity);
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

        public static PositionComponent GetPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static PositionComponent GetSecondaryPositionComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).PositionComponentPool.GetTyped(entity);
        }

        public static PositionComponent GetOrAddPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasPositionComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryPositionComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).PositionComponentPool.HasTyped(entity);
        }

        public static void SetPositionComponent(this VenusEntity entity, PositionComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static PositionComponent AddPositionComponent(this VenusEntity entity, PositionComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).PositionComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static SpawnedEntityComponent AddSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            return pool.AddTyped(entity);
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

        public static SpawnedEntityComponent GetSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static SpawnedEntityComponent GetSecondarySpawnedEntityComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).SpawnedEntityComponentPool.GetTyped(entity);
        }

        public static SpawnedEntityComponent GetOrAddSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasSpawnedEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondarySpawnedEntityComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).SpawnedEntityComponentPool.HasTyped(entity);
        }

        public static void SetSpawnedEntityComponent(this VenusEntity entity, SpawnedEntityComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static SpawnedEntityComponent AddSpawnedEntityComponent(this VenusEntity entity, SpawnedEntityComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).SpawnedEntityComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static TagComponent AddTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            return pool.AddTyped(entity);
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

        public static TagComponent GetTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static TagComponent GetSecondaryTagComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).TagComponentPool.GetTyped(entity);
        }

        public static TagComponent GetOrAddTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasTagComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryTagComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).TagComponentPool.HasTyped(entity);
        }

        public static void SetTagComponent(this VenusEntity entity, TagComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static TagComponent AddTagComponent(this VenusEntity entity, TagComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).TagComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static TestComponent AddTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            return pool.AddTyped(entity);
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

        public static TestComponent GetTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static TestComponent GetSecondaryTestComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).TestComponentPool.GetTyped(entity);
        }

        public static TestComponent GetOrAddTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasTestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryTestComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).TestComponentPool.HasTyped(entity);
        }

        public static void SetTestComponent(this VenusEntity entity, TestComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static TestComponent AddTestComponent(this VenusEntity entity, TestComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).TestComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static VenusEntityComponent AddVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            return pool.AddTyped(entity);
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

        public static VenusEntityComponent GetVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static VenusEntityComponent GetSecondaryVenusEntityComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).VenusEntityComponentPool.GetTyped(entity);
        }

        public static VenusEntityComponent GetOrAddVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasVenusEntityComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryVenusEntityComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).VenusEntityComponentPool.HasTyped(entity);
        }

        public static void SetVenusEntityComponent(this VenusEntity entity, VenusEntityComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static VenusEntityComponent AddVenusEntityComponent(this VenusEntity entity, VenusEntityComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).VenusEntityComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static OwnershipComponent AddOwnershipComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipComponentPool;
            return pool.AddTyped(entity);
        }

        public static void DelOwnershipComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsOwnershipComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static OwnershipComponent GetOwnershipComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static OwnershipComponent GetSecondaryOwnershipComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).OwnershipComponentPool.GetTyped(entity);
        }

        public static OwnershipComponent GetOrAddOwnershipComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasOwnershipComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryOwnershipComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).OwnershipComponentPool.HasTyped(entity);
        }

        public static void SetOwnershipComponent(this VenusEntity entity, OwnershipComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static OwnershipComponent AddOwnershipComponent(this VenusEntity entity, OwnershipComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static OwnershipRequestComponent AddOwnershipRequestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipRequestComponentPool;
            return pool.AddTyped(entity);
        }

        public static void DelOwnershipRequestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipRequestComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsOwnershipRequestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipRequestComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static OwnershipRequestComponent GetOwnershipRequestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipRequestComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static OwnershipRequestComponent GetSecondaryOwnershipRequestComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).OwnershipRequestComponentPool.GetTyped(entity);
        }

        public static OwnershipRequestComponent GetOrAddOwnershipRequestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipRequestComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasOwnershipRequestComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipRequestComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryOwnershipRequestComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).OwnershipRequestComponentPool.HasTyped(entity);
        }

        public static void SetOwnershipRequestComponent(this VenusEntity entity, OwnershipRequestComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipRequestComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static OwnershipRequestComponent AddOwnershipRequestComponent(this VenusEntity entity, OwnershipRequestComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).OwnershipRequestComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static InputRecieverComponent AddInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).InputRecieverComponentPool;
            return pool.AddTyped(entity);
        }

        public static void DelInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).InputRecieverComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).InputRecieverComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static InputRecieverComponent GetInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).InputRecieverComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static InputRecieverComponent GetSecondaryInputRecieverComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).InputRecieverComponentPool.GetTyped(entity);
        }

        public static InputRecieverComponent GetOrAddInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).InputRecieverComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).InputRecieverComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryInputRecieverComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).InputRecieverComponentPool.HasTyped(entity);
        }

        public static void SetInputRecieverComponent(this VenusEntity entity, InputRecieverComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).InputRecieverComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static InputRecieverComponent AddInputRecieverComponent(this VenusEntity entity, InputRecieverComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).InputRecieverComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }

        public static LocalInputRecieverComponent AddLocalInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).LocalInputRecieverComponentPool;
            return pool.AddTyped(entity);
        }

        public static void DelLocalInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).LocalInputRecieverComponentPool;
            pool.RemoveTyped(entity);
        }

        public static void DelIfExistsLocalInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).LocalInputRecieverComponentPool;
            if (pool.HasTyped(entity))
            {
                pool.RemoveTyped(entity);
            }
        }

        public static LocalInputRecieverComponent GetLocalInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).LocalInputRecieverComponentPool;
            return pool.GetTyped(entity);
        }
        
        public static LocalInputRecieverComponent GetSecondaryLocalInputRecieverComponent(this VenusEntity entity)
        {
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).LocalInputRecieverComponentPool.GetTyped(entity);
        }

        public static LocalInputRecieverComponent GetOrAddLocalInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).LocalInputRecieverComponentPool;
            if (pool.HasTyped(entity))
            {
                return pool.GetTyped(entity);
            }
            return pool.AddTyped(entity);
        }

        public static bool HasLocalInputRecieverComponent(this VenusEntity entity)
        {
            var pool = ((VenusPools)Venus.Pools).LocalInputRecieverComponentPool;
            return pool.HasTyped(entity);
        }
        
        public static bool HasSecondaryLocalInputRecieverComponent(this VenusEntity entity)
        {
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {
                return false;
            }
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).LocalInputRecieverComponentPool.HasTyped(entity);
        }

        public static void SetLocalInputRecieverComponent(this VenusEntity entity, LocalInputRecieverComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).LocalInputRecieverComponentPool;
            pool.SetTyped(entity, component);
        }
        
        public static LocalInputRecieverComponent AddLocalInputRecieverComponent(this VenusEntity entity, LocalInputRecieverComponent component)
        {
            var pool = ((VenusPools)Venus.Pools).LocalInputRecieverComponentPool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }


    }
}
