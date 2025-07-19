using System;
using Unity.Collections.LowLevel.Unsafe;
using VenusECS.Core.Reflection.Attributes;
using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public class VenusPool<T> : IVenusPool
        where T : struct, IVenusComponent
    {
        private int _poolSizePowerOfTwo = 10;
        private int _bitsPerInt = 32;
        private int[] _bitmask;
        private T[] _components;
        private int _minEntityId = int.MaxValue;
        private int _maxEntityId = int.MinValue;
        private int _capacity;

        public int[] Bitmask => _bitmask;
        public int MinEntityId => _minEntityId;
        public int MaxEntityId => _maxEntityId;
        public int Length => _maxEntityId >= _minEntityId ? _maxEntityId - _minEntityId + 1 : 0;
        public int BitsPerInt => _bitsPerInt;

        public event Action<IVenusPool, VenusEntity> OnEntityAdded;
        public event Action<IVenusPool, VenusEntity> OnEntityRemoved;

        public VenusPool()
        {
            int size = 1 << _poolSizePowerOfTwo;

            var componentType = typeof(T);
            var customAttributes = componentType.GetCustomAttributes(false);
            foreach (var attribute in customAttributes)
            {
                if (attribute is PoolSizeAttribute poolSizeAttribute)
                {
                    size = poolSizeAttribute.PoolSize;
                    break;
                }
            }

            _capacity = size;
            _bitmask = new int[size / _bitsPerInt];
            _components = new T[size];
        }

        private void EnsureCapacity(int entityId)
        {
            if (entityId < _minEntityId) _minEntityId = entityId;
            if (entityId > _maxEntityId) _maxEntityId = entityId;

            int requiredCapacity = (_maxEntityId - _minEntityId + 1);
            if (requiredCapacity > _capacity)
            {
                int newCapacity = Math.Max(_capacity * 2, requiredCapacity);
                Array.Resize(ref _bitmask, newCapacity / _bitsPerInt);
                Array.Resize(ref _components, newCapacity);
                _capacity = newCapacity;
            }
        }

        private int GetBitmaskIndex(int entityId)
        {
            return (entityId - _minEntityId) / _bitsPerInt;
        }

        private int GetBitPosition(int entityId)
        {
            return (entityId - _minEntityId) % _bitsPerInt;
        }

        public ref T1 Get<T1>(VenusEntity entity) where T1 : struct, IVenusComponent
        {
#if SAFETY_CHECKS
            if (!Has<T1>(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id} Gen {entity.Generation}");
#endif        
            return ref UnsafeUtility.As<T, T1>(ref _components[entity.Id - _minEntityId]);
        }

        public ref T1 Add<T1>(VenusEntity entity) where T1 : struct, IVenusComponent
        {
#if SAFETY_CHECKS
            if (Has<T1>(entity)) throw new ArgumentException($"Component of type already exists. Entity Id {entity.Id} Gen {entity.Generation}");
#endif
            EnsureCapacity(entity.Id);
            int bitmaskIndex = GetBitmaskIndex(entity.Id);
            int bitPosition = GetBitPosition(entity.Id);
            _bitmask[bitmaskIndex] |= (1 << bitPosition);
            _components[entity.Id - _minEntityId] = new T();
            OnEntityAdded?.Invoke(this, entity);
            return ref UnsafeUtility.As<T, T1>(ref _components[entity.Id - _minEntityId]);
        }

        public void Remove(VenusEntity entity)
        {
#if SAFETY_CHECKS
            if (!HasTyped(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id} Gen {entity.Generation}");
#endif        
            int bitmaskIndex = GetBitmaskIndex(entity.Id);
            int bitPosition = GetBitPosition(entity.Id);
            _bitmask[bitmaskIndex] &= ~(1 << bitPosition);
            OnEntityRemoved?.Invoke(this, entity);
        }

        public bool Has<T1>(VenusEntity entity) where T1 : struct, IVenusComponent
        {
            if (entity.Id < _minEntityId || entity.Id > _maxEntityId) return false;
            int bitmaskIndex = GetBitmaskIndex(entity.Id);
            int bitPosition = GetBitPosition(entity.Id);
            return (_bitmask[bitmaskIndex] & (1 << bitPosition)) != 0;
        }

        public void Clear()
        {
            Array.Clear(_bitmask, 0, _bitmask.Length);
            _minEntityId = int.MaxValue;
            _maxEntityId = int.MinValue;
        }

        public ref T GetTyped(VenusEntity entity)
        {
            return ref _components[entity.Id - _minEntityId];
        }

        public ref T AddTyped(VenusEntity entity)
        {
            EnsureCapacity(entity.Id);
            int bitmaskIndex = GetBitmaskIndex(entity.Id);
            int bitPosition = GetBitPosition(entity.Id);
            _bitmask[bitmaskIndex] |= (1 << bitPosition);
            _components[entity.Id - _minEntityId] = new T();
            OnEntityAdded?.Invoke(this, entity);
            return ref _components[entity.Id - _minEntityId];
        }

        public void RemoveTyped(VenusEntity entity)
        {
            int bitmaskIndex = GetBitmaskIndex(entity.Id);
            int bitPosition = GetBitPosition(entity.Id);
            _bitmask[bitmaskIndex] &= ~(1 << bitPosition);
            OnEntityRemoved?.Invoke(this, entity);
        }

        public bool HasTyped(VenusEntity entity)
        {
            if (entity.Id < _minEntityId || entity.Id > _maxEntityId) return false;
            int bitmaskIndex = GetBitmaskIndex(entity.Id);
            int bitPosition = GetBitPosition(entity.Id);
            return (_bitmask[bitmaskIndex] & (1 << bitPosition)) != 0;
        }

        public IEnumerable<VenusEntity> GetAllEntities()
        {
            for (int i = 0; i < _bitmask.Length; i++)
            {
                int mask = _bitmask[i];
                if (mask == 0) continue;

                for (int bit = 0; bit < _bitsPerInt; bit++)
                {
                    if ((mask & (1 << bit)) != 0)
                    {
                        int entityId = _minEntityId + (i * _bitsPerInt) + bit;
                        yield return new VenusEntity { Id = entityId };
                    }
                }
            }
        }
    }
}