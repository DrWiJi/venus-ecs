using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VenusECS.Core.Pool;

namespace VenusECS.Core
{

    //TODO: Resolve dependencies to this class through some service locator
    public static class Venus
    {
        private static IVenusPools _pools;
        private static IVenusPoolsFactory _poolsFactory;

        public static IVenusPools Pools => _pools;
        
        public static readonly List<IVenusPools> SecondaryPools = new();

        public static int CurrentSecondaryPoolIndex = 0;
        public static Dictionary<int, Dictionary<VenusEntity, VenusEntity>> PrimaryWorldEntityToSecondaryWorldEntity = new();
        public static Dictionary<int, Dictionary<VenusEntity, VenusEntity>> SecondaryWorldEntityToPrimaryWorldEntity = new();

        static Venus()
        {
            InitializeMainPools();
        }

        private static void InitializeMainPools()
        {
            var factoryType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && !type.IsInterface)
                .FirstOrDefault(type => typeof(IVenusPoolsFactory).IsAssignableFrom(type));

            if (factoryType == null)
            {
                throw new InvalidOperationException("No implementation of IVenusPoolsFactory found in current domain");
            }

            var factory = (IVenusPoolsFactory)Activator.CreateInstance(factoryType);
            _pools = factory.Create();
            _poolsFactory = factory;
            _pools.OnEntityCreated += OnEntityCreated;
            _pools.OnEntityDeleted += OnEntityDeleted;
            _pools.OnEntityCreatedExternally += OnSecondaryEntityCreated;
            _pools.OnEntityDeletedExternally += OnSecondaryEntityDeleted;
        }

        private static void OnEntityDeleted(VenusEntity entity)
        {

        }

        private static void OnEntityCreated(VenusEntity entity)
        {
        }

        private static void OnSecondaryEntityDeleted(int worldIndex, VenusEntity entity)
        {
            if(SecondaryWorldEntityToPrimaryWorldEntity[worldIndex].ContainsKey(entity))
            {
                var primaryEntity = SecondaryWorldEntityToPrimaryWorldEntity[worldIndex][entity];
                PrimaryWorldEntityToSecondaryWorldEntity[worldIndex].Remove(primaryEntity);
                SecondaryWorldEntityToPrimaryWorldEntity[worldIndex].Remove(entity);
                _pools.DeleteEntity(primaryEntity);
            }
            else
            {
                throw new Exception($"Entity {entity.Id} not found in the secondary world!");
            }
        }

        private static void OnSecondaryEntityCreated(int worldIndex, VenusEntity entity)
        {
            if(!SecondaryWorldEntityToPrimaryWorldEntity[worldIndex].ContainsKey(entity))
            {
                var primaryEntity = _pools.CreateEntity();
                SecondaryWorldEntityToPrimaryWorldEntity[worldIndex].Add(entity, primaryEntity);
                PrimaryWorldEntityToSecondaryWorldEntity[worldIndex].Add(primaryEntity, entity);
            }
            else
            {
                throw new Exception($"Entity {entity.Id} already exists in the primary world!");
            }
        }

        public static int CreateSecondaryPools()
        {
            SecondaryPools.Add(_poolsFactory.Create());
            int worldIndex = SecondaryPools.Count - 1;
            PrimaryWorldEntityToSecondaryWorldEntity.Add(worldIndex, new Dictionary<VenusEntity, VenusEntity>());
            SecondaryWorldEntityToPrimaryWorldEntity.Add(worldIndex, new Dictionary<VenusEntity, VenusEntity>());
            SecondaryPools[worldIndex].OnEntityCreatedExternally += OnSecondaryEntityCreated;
            SecondaryPools[worldIndex].OnEntityDeletedExternally += OnSecondaryEntityDeleted;
            SecondaryPools[worldIndex].WorldIndex = worldIndex;
            return worldIndex;
        }

        public static void RemoveSecondaryPool(int index)
        {
            SecondaryPools.RemoveAt(index);
            SecondaryPools[index].OnEntityCreatedExternally -= OnSecondaryEntityCreated;
            SecondaryPools[index].OnEntityDeletedExternally -= OnSecondaryEntityDeleted;
        }

        public static void SwitchSecondaryPool(int index)
        {
            CurrentSecondaryPoolIndex = index;
        }

        public static void Reset()
        {
            _pools.Clear();
            PrimaryWorldEntityToSecondaryWorldEntity.Clear();
            SecondaryWorldEntityToPrimaryWorldEntity.Clear();
            foreach (var pool in SecondaryPools)
            {
                pool.Clear();
                pool.OnEntityCreatedExternally -= OnSecondaryEntityCreated;
                pool.OnEntityDeletedExternally -= OnSecondaryEntityDeleted;
            }
            SecondaryPools.Clear();
        }
    }
}