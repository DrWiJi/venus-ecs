using System;
using System.Collections.Generic;
using System.Linq;
using VenusECS.Core.Pool;

namespace VenusECS.Core
{
    public abstract class VenusPoolsBase : IVenusPools
    {
        protected readonly Dictionary<Type, IVenusPool> _pools = new ();
        
        protected HashSet<VenusEntity> _entities;
        protected Dictionary<VenusEntity, HashSet<IVenusPool>> _usedPools;
        protected int _entitiesCounter = 0;
        protected Queue<VenusEntity> _freeEntities;

        public VenusPoolsBase(int capacity)
        {
            _entities = new(capacity);
            _freeEntities = new Queue<VenusEntity>(capacity);
            _usedPools = new(capacity);
            foreach (var pool in _pools)
            {
                pool.Value.OnEntityAdded += AddComponentHandler;
                pool.Value.OnEntityRemoved += RemoveComponentHandler;
            }
            
            for (int i = 0; i < capacity; i++)
            {
                var newEntity = new VenusEntity();
                newEntity.Id = _entitiesCounter++;
                newEntity.Generation = 0;
                _freeEntities.Enqueue(newEntity);
                _usedPools.Add(newEntity, new HashSet<IVenusPool>(_pools.Count));
            }
        }

        private void RemoveComponentHandler(IVenusPool pool, VenusEntity entity)
        {
            _usedPools[entity].Remove(pool);
        }

        private void AddComponentHandler(IVenusPool pool, VenusEntity entity)
        {
            _usedPools[entity].Add(pool);
        }

        public VenusEntity CreateEntity()
        {
            if (_freeEntities.Count == 0)
            {
                var entity = new VenusEntity {Id = _entitiesCounter++, Generation = 0};
                _entities.Add(entity);
                _usedPools.Add(entity, new HashSet<IVenusPool>(64));
                return entity;
            }
            else
            {
                var entity = _freeEntities.Dequeue();
                _entities.Add(entity);
                return entity;
            }
        }

        public void DeleteEntity(VenusEntity entity)
        {
            DelEntityFromAllPools(entity);
            _usedPools[entity].Clear();
            _entities.Remove(entity);
            _freeEntities.Enqueue(entity);
        }

        public virtual void Clear()
        {
            var entitiesList = _entities.ToList();
            for (int i = 0; i < _entities.Count; i++)
            {
                var newEntity = entitiesList[i];
                newEntity.Generation = 0;

                _freeEntities.Enqueue(newEntity);
                DelEntityFromAllPools(newEntity);
                _usedPools[newEntity].Clear();
            }
            _entities.Clear();
            foreach (var pool in _pools)
            {
                pool.Value.Clear();
            }
        }

        private void DelEntityFromAllPools(VenusEntity newEntity)
        {
            foreach (var pool in _usedPools[newEntity])
            {
                pool.Remove(newEntity);
            }
        }

        public abstract IVenusPool GetPool<T1>() where T1 : struct, IVenusComponent;
    }
}