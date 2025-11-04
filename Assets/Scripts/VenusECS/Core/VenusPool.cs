using System;
using Unity.Collections.LowLevel.Unsafe;
using VenusECS.Core.Reflection.Attributes;
using System.Collections.Generic;
using MessagePack;
using System.Reflection;

namespace VenusECS.Core.Pool
{
    public class VenusPool<T> : IVenusPool
        where T : struct, IVenusComponent
    {
        private const int SnapshotVersion = 1;
        private int _poolSizePowerOfTwo = 10;
        private int _bitsPerInt = 32;
        private int[] _bitmask;
        private T[] _components;
        private Type _type;
        private int _minEntityId = int.MaxValue;
        private int _maxEntityId = int.MinValue;
        private int _capacity;
        private int _poolIndex;

        public int[] Bitmask => _bitmask;
        public int MinEntityId => _minEntityId;
        public int MaxEntityId => _maxEntityId;
        public int Length => _maxEntityId >= _minEntityId ? _maxEntityId - _minEntityId + 1 : 0;
        public int BitsPerInt => _bitsPerInt;
        public int PoolIndex;

        public event Action<IVenusPool, VenusEntity> OnEntityAdded;
        public event Action<IVenusPool, VenusEntity> OnEntityRemoved;
        public event Action<IVenusPool, VenusEntity, IVenusComponent> OnComponentChanged;

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
            _type = typeof(T);
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

        public T1 Get<T1>(VenusEntity entity) where T1 : struct, IVenusComponent
        {
#if SAFETY_CHECKS
            if (!Has<T1>(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id}");
#endif        
            return UnsafeUtility.As<T, T1>(ref _components[entity.Id - _minEntityId]);
        }

        public T1 Add<T1>(VenusEntity entity) where T1 : struct, IVenusComponent
        {
#if SAFETY_CHECKS
            if (Has<T1>(entity)) throw new ArgumentException($"Component of type already exists. Entity Id {entity.Id}");
#endif
            EnsureCapacity(entity.Id);
            int bitmaskIndex = GetBitmaskIndex(entity.Id);
            int bitPosition = GetBitPosition(entity.Id);
            _bitmask[bitmaskIndex] |= (1 << bitPosition);
            int index = entity.Id - _minEntityId;
            _components[index] = new T();
            OnEntityAdded?.Invoke(this, entity);
            OnComponentChanged?.Invoke(this, entity, _components[index]);
            return UnsafeUtility.As<T, T1>(ref _components[index]);
        }

        public void Remove(VenusEntity entity)
        {
#if SAFETY_CHECKS
            if (!HasTyped(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id}");
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

        public T GetTyped(VenusEntity entity)
        {
            return _components[entity.Id - _minEntityId];
        }

        public T AddTyped(VenusEntity entity)
        {
#if SAFETY_CHECKS
            if (HasTyped(entity)) throw new ArgumentException($"Component of type already exists. Entity Id {entity.Id}");
#endif
            EnsureCapacity(entity.Id);
            int bitmaskIndex = GetBitmaskIndex(entity.Id);
            int bitPosition = GetBitPosition(entity.Id);
            _bitmask[bitmaskIndex] |= (1 << bitPosition);
            _components[entity.Id - _minEntityId] = new T();
            OnEntityAdded?.Invoke(this, entity);
            return _components[entity.Id - _minEntityId];
        }

        public void RemoveTyped(VenusEntity entity)
        {
#if SAFETY_CHECKS
            if (!HasTyped(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id}");
#endif
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

        public void SetTyped(VenusEntity entity, T value)
        {
#if SAFETY_CHECKS
            if (!HasTyped(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id}. Pool {typeof(T).Name}");
#endif
            _components[entity.Id - _minEntityId] = value;
            OnComponentChanged?.Invoke(this, entity, _components[entity.Id - _minEntityId]);
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

        public void Set<T1>(VenusEntity entity, T1 value) where T1 : struct, IVenusComponent
        {
#if SAFETY_CHECKS
            if (!Has<T1>(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id}. Pool {typeof(T).Name}");
#endif
            var index = entity.Id - _minEntityId;
            var oldValue = _components[index];
            if (oldValue.Equals(value)) return;
            _components[index] = UnsafeUtility.As<T1, T>(ref value);
            OnComponentChanged?.Invoke(this, entity, _components[index]);
        }

        public Type GetComponentType()
        {
            return _type;
        }

        public unsafe void* GetPointer(VenusEntity entity)
        {
#if SAFETY_CHECKS
            if (!HasTyped(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id}. Pool {typeof(T).Name}");
#endif
            return UnsafeUtility.AddressOf(ref _components[entity.Id - _minEntityId]);
        }

        public int GetPoolIndex()
        {
            return _poolIndex;
        }

        public unsafe void SetRaw(VenusEntity entity, byte* dataPtr)
        {
#if SAFETY_CHECKS
            if (!HasTyped(entity)) throw new ArgumentException($"Component on entity doesn't exists. Entity Id {entity.Id}. Pool {typeof(T).Name}");
#endif
            unsafe
            {
                UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref _components[entity.Id - _minEntityId]), dataPtr, UnsafeUtility.SizeOf(typeof(T)));
            }
        }

        public unsafe void AddRaw(VenusEntity entity, byte* dataPtr)
        {
#if SAFETY_CHECKS
            if (HasTyped(entity)) throw new ArgumentException($"Component of type already exists. Entity Id {entity.Id}");
#endif
            unsafe
            {
                UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref _components[entity.Id - _minEntityId]), dataPtr, UnsafeUtility.SizeOf(typeof(T)));
            }
        }

        public byte[] GetSnapshot()
        {
            //If component type is not serializable, return empty snapshot
            if (typeof(T).GetCustomAttribute<MessagePackObjectAttribute>() == null)
                return Array.Empty<byte>();

            // Header layout (int32 each):
            // [0] version
            // [1] bitsPerInt
            // [2] minEntityId
            // [3] maxEntityId
            // [4] capacity (components length)
            // [5] poolIndex
            // [6] bitmaskLength (number of ints)
            // [7] componentsLength (number of T items)
            // [8] componentSize (bytes)
            int bitmaskLength = _bitmask?.Length ?? 0;
            int componentsLength = _components?.Length ?? 0;
            int componentSize = UnsafeUtility.SizeOf(typeof(T));

            const int headerInts = 9;
            int headerBytes = headerInts * sizeof(int);
            int bitmaskBytes = bitmaskLength * sizeof(int);
            int componentsBytes = componentsLength * componentSize;
            int totalSize = headerBytes + bitmaskBytes + componentsBytes;

            var snapshot = new byte[totalSize];

            // Write header
            int[] header = new int[headerInts]
            {
                SnapshotVersion,
                _bitsPerInt,
                _minEntityId,
                _maxEntityId,
                _capacity,
                _poolIndex,
                bitmaskLength,
                componentsLength,
                componentSize
            };
            Buffer.BlockCopy(header, 0, snapshot, 0, headerBytes);

            int offset = headerBytes;

            // Write bitmask
            if (bitmaskLength > 0)
            {
                unsafe
                {
                    fixed (byte* dst = &snapshot[offset])
                    {
                        void* src = UnsafeUtility.AddressOf(ref _bitmask[0]);
                        UnsafeUtility.MemCpy(dst, src, bitmaskBytes);
                    }
                }
                offset += bitmaskBytes;
            }

            // Write components
            if (componentsLength > 0)
            {
                for (int i = 0; i < componentsLength; i++)
                {
                    var srcBytes = MessagePackSerializer.Serialize(_components[i]);
                    Buffer.BlockCopy(srcBytes, 0, snapshot, offset, srcBytes.Length);
                    offset += srcBytes.Length;
                }
            }
            return snapshot;
        }

        public void RestoreSnapshot(byte[] snapshot)
        {
            //If component type is not MessagePackObject, return
            if (typeof(T).GetCustomAttribute<MessagePackObjectAttribute>() == null)
                return;

            if (snapshot == null || snapshot.Length == 0) return;

            const int headerInts = 9;
            int headerBytes = headerInts * sizeof(int);
            if (snapshot.Length < headerBytes) throw new ArgumentException("Snapshot is too small");

            int[] header = new int[headerInts];
            Buffer.BlockCopy(snapshot, 0, header, 0, headerBytes);

            int version = header[0];
            int bitsPerInt = header[1];
            int minEntityId = header[2];
            int maxEntityId = header[3];
            int capacity = header[4];
            int poolIndex = header[5];
            int bitmaskLength = header[6];
            int componentsLength = header[7];
            int componentSize = header[8];

            if (version != SnapshotVersion)
            {
                throw new ArgumentException($"Unsupported snapshot version: {version}");
            }

            if (bitsPerInt != _bitsPerInt)
            {
                // We can still restore, but this pool expects specific bitsPerInt; mismatch indicates incompatible runtime
                throw new ArgumentException("Snapshot bitsPerInt mismatch");
            }

            if (componentSize != UnsafeUtility.SizeOf(typeof(T)))
            {
                throw new ArgumentException("Snapshot component size mismatch");
            }

            int bitmaskBytes = bitmaskLength * sizeof(int);
            int componentsBytes = componentsLength * componentSize;
            int required = headerBytes + bitmaskBytes + componentsBytes;
            if (snapshot.Length < required) throw new ArgumentException("Snapshot data truncated");

            _minEntityId = minEntityId;
            _maxEntityId = maxEntityId;
            _capacity = capacity;
            _poolIndex = poolIndex;

            if (bitmaskLength > 0)
            {
                if (_bitmask == null || _bitmask.Length != bitmaskLength)
                {
                    _bitmask = new int[bitmaskLength];
                }
            }
            else
            {
                _bitmask = Array.Empty<int>();
            }

            if (componentsLength > 0)
            {
                if (_components == null || _components.Length != componentsLength)
                {
                    _components = new T[componentsLength];
                }
            }
            else
            {
                _components = Array.Empty<T>();
            }

            int offset = headerBytes;

            // Read bitmask
            if (bitmaskLength > 0)
            {
                unsafe
                {
                    fixed (byte* src = &snapshot[offset])
                    {
                        void* dst = UnsafeUtility.AddressOf(ref _bitmask[0]);
                        UnsafeUtility.MemCpy(dst, src, bitmaskBytes);
                    }
                }
                offset += bitmaskBytes;
            }

            // Read components
            if (componentsLength > 0)
            {
                for(int i = 0; i < componentsLength; i++)
                {
                    var srcBytes = new byte[componentSize];
                    Buffer.BlockCopy(snapshot, offset, srcBytes, 0, componentSize);
                    _components[i] = MessagePackSerializer.Deserialize<T>(srcBytes);
                    offset += componentSize;
                }
            }
        }
    }
}