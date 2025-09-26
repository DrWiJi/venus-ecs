using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using VenusECS.Core.Pool;

namespace VenusECS.Core
{
    public struct PoolsDeltaPortion
    {
        public enum EAction
        {
            Add,
            Remove,
            Change
        }
        public EAction Action;
        public VenusEntity Entity;
        public int PoolIndex;
        public int DataStartIndex;
        public int DataLength;
    }

    public abstract class VenusPoolsBase : IVenusPools
    {
        private const int SnapshotVersion = 1;
        protected readonly Dictionary<Type, IVenusPool> _pools = new();
        protected IVenusPool[] _poolsArray;

        protected HashSet<VenusEntity> _entities;
        protected Dictionary<VenusEntity, HashSet<IVenusPool>> _usedPools;
        protected int _entitiesCounter = 0;
        protected Queue<VenusEntity> _freeEntities;

        protected PoolsDeltaPortion[] _deltaPortions;
        protected int _deltaPortionsCounter = 0;
        protected byte[] _deltaPortionsData = new byte[1024];
        protected int _deltaPortionsDataCounter = 0;

        public VenusPoolsBase(int capacity)
        {
            _entities = new(capacity);
            _freeEntities = new Queue<VenusEntity>(capacity);
            _usedPools = new(capacity);
            foreach (var pool in _pools)
            {
                pool.Value.OnEntityAdded += AddComponentHandler;
                pool.Value.OnEntityRemoved += RemoveComponentHandler;
                pool.Value.OnComponentChanged += ChangeComponentHandler;
            }

            for (int i = 0; i < capacity; i++)
            {
                var newEntity = new VenusEntity();
                newEntity.Id = _entitiesCounter++;
                _freeEntities.Enqueue(newEntity);
                _usedPools.Add(newEntity, new HashSet<IVenusPool>(_pools.Count));
            }
        }

        private void ChangeComponentHandler(IVenusPool pool, VenusEntity entity, IVenusComponent component)
        {
            _deltaPortions[_deltaPortionsCounter].Action = PoolsDeltaPortion.EAction.Change;
            _deltaPortions[_deltaPortionsCounter].Entity = entity;
            _deltaPortions[_deltaPortionsCounter].DataStartIndex = _deltaPortionsDataCounter;
            _deltaPortions[_deltaPortionsCounter].DataLength = UnsafeUtility.SizeOf(pool.GetComponentType());
            EnsureCapacity(_deltaPortionsDataCounter + _deltaPortions[_deltaPortionsCounter].DataLength);
            unsafe
            {
                fixed (byte* ptr = &_deltaPortionsData[_deltaPortions[_deltaPortionsCounter].DataStartIndex])
                {
                    UnsafeUtility.MemCpy(ptr, pool.GetPointer(entity), _deltaPortions[_deltaPortionsCounter].DataLength);
                }
            }
            _deltaPortionsDataCounter += _deltaPortions[_deltaPortionsCounter].DataLength;
            _deltaPortionsCounter++;
        }

        private void EnsureCapacity(int size)
        {
            if (_deltaPortionsDataCounter + size > _deltaPortionsData.Length)
            {
                Array.Resize(ref _deltaPortionsData, _deltaPortionsData.Length * 2);
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
                var entity = new VenusEntity { Id = _entitiesCounter++ };
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

        public (PoolsDeltaPortion[], byte[]) FlushDelta()
        {
            _deltaPortionsCounter = 0;
            _deltaPortionsDataCounter = 0;
            return (_deltaPortions, _deltaPortionsData);
        }

        public void ApplyDelta(PoolsDeltaPortion[] deltaPortions, byte[] deltaPortionsData)
        {
            FillPoolsArray();
            for (int i = 0; i < deltaPortions.Length; i++)
            {
                switch (deltaPortions[i].Action)
                {
                    case PoolsDeltaPortion.EAction.Add:
                        unsafe
                        {
                            fixed (byte* ptr = &deltaPortionsData[deltaPortions[i].DataStartIndex])
                            {
                                _poolsArray[deltaPortions[i].PoolIndex].AddRaw(deltaPortions[i].Entity, ptr);
                            }
                        }
                        break;
                    case PoolsDeltaPortion.EAction.Remove:
                        _poolsArray[deltaPortions[i].PoolIndex].Remove(deltaPortions[i].Entity);
                        break;
                    case PoolsDeltaPortion.EAction.Change:
                        unsafe
                        {
                            fixed (byte* ptr = &deltaPortionsData[deltaPortions[i].DataStartIndex])
                            {
                                _poolsArray[deltaPortions[i].PoolIndex].SetRaw(deltaPortions[i].Entity, ptr);
                            }
                        }
                        break;
                }
            }
        }

        private void FillPoolsArray()
        {
            if (_poolsArray == null)
            {
                _poolsArray = new IVenusPool[_pools.Count];
                foreach (var pool in _pools)
                {
                    _poolsArray[pool.Value.GetPoolIndex()] = pool.Value;
                }
            }
        }

        public abstract IVenusPool GetPool<T1>() where T1 : struct, IVenusComponent;

        public byte[] GetSnapshot()
        {
            // Header layout (int32 each):
            // [0] version
            // [1] poolsCount

            FillPoolsArray();
            int poolsCount = _poolsArray.Length;
            int perPoolHeaderInts = 2; // poolIndex, snapshotLength
            int headerInts = 2;
            int headerBytes = headerInts * sizeof(int);
            int perPoolHeaderBytes = perPoolHeaderInts * sizeof(int);

            int poolsSectionBytes = 0;
            var poolSnapshots = new byte[poolsCount][];
            for (int i = 0; i < poolsCount; i++)
            {
                var pool = _poolsArray[i];
                var snapshot = pool?.GetSnapshot() ?? Array.Empty<byte>();
                poolSnapshots[i] = snapshot;
                poolsSectionBytes += perPoolHeaderBytes + snapshot.Length;
            }

            int totalBytes = headerBytes + poolsSectionBytes;
            var snapshotBytes = new byte[totalBytes];

            // Write header
            int[] header = new int[]
            {
                SnapshotVersion,
                poolsCount
            };
            Buffer.BlockCopy(header, 0, snapshotBytes, 0, headerBytes);

            int offset = headerBytes;

            // Write pools
            for (int i = 0; i < poolsCount; i++)
            {
                int poolIndex = i;
                byte[] data = poolSnapshots[i] ?? Array.Empty<byte>();

                // per-pool header
                int[] perPoolHeader = new int[] { poolIndex, data.Length };
                Buffer.BlockCopy(perPoolHeader, 0, snapshotBytes, offset, perPoolHeaderBytes);
                offset += perPoolHeaderBytes;

                // data
                if (data.Length > 0)
                {
                    Buffer.BlockCopy(data, 0, snapshotBytes, offset, data.Length);
                    offset += data.Length;
                }
            }

            return snapshotBytes;
        }

        public void RestoreSnapshot(byte[] snapshot)
        {
            if (snapshot == null || snapshot.Length == 0) return;

            // Read header
            int headerInts = 2;
            int headerBytes = headerInts * sizeof(int);
            if (snapshot.Length < headerBytes) throw new ArgumentException("Snapshot is too small");

            int[] header = new int[headerInts];
            Buffer.BlockCopy(snapshot, 0, header, 0, headerBytes);

            int version = header[0];
            int poolsCount = header[1];

            if (version != SnapshotVersion)
            {
                throw new ArgumentException($"Unsupported pools snapshot version: {version}");
            }

            int offset = headerBytes;

            // Ensure mapping by pool index
            FillPoolsArray();

            // Read and restore each pool
            int perPoolHeaderInts = 2;
            int perPoolHeaderBytes = perPoolHeaderInts * sizeof(int);
            for (int i = 0; i < poolsCount; i++)
            {
                if (snapshot.Length < offset + perPoolHeaderBytes) throw new ArgumentException("Snapshot data truncated (pool header)");
                int[] perPoolHeader = new int[perPoolHeaderInts];
                Buffer.BlockCopy(snapshot, offset, perPoolHeader, 0, perPoolHeaderBytes);
                offset += perPoolHeaderBytes;

                int poolIndex = perPoolHeader[0];
                int dataLength = perPoolHeader[1];

                if (snapshot.Length < offset + dataLength) throw new ArgumentException("Snapshot data truncated (pool data)");

                var data = new byte[dataLength];
                if (dataLength > 0)
                {
                    Buffer.BlockCopy(snapshot, offset, data, 0, dataLength);
                    offset += dataLength;
                }

                var pool = _poolsArray[poolIndex];
                pool.RestoreSnapshot(data);
            }

            // Rebuild buffers from pools data
            // 1) Determine active ids and max id
            int maxEntityId = -1;
            var activeIdsSet = new HashSet<int>(1024);
            for (int p = 0; p < _poolsArray.Length; p++)
            {
                var pool = _poolsArray[p];
                var bitmask = pool.Bitmask;
                int bitsPerInt = pool.BitsPerInt;
                int minId = pool.MinEntityId;
                int localMax = pool.MaxEntityId;
                if (localMax > maxEntityId) maxEntityId = localMax;
                for (int i = 0; i < bitmask.Length; i++)
                {
                    int mask = bitmask[i];
                    if (mask == 0) continue;
                    for (int b = 0; b < bitsPerInt; b++)
                    {
                        if ((mask & (1 << b)) != 0)
                        {
                            int entityId = minId + (i * bitsPerInt) + b;
                            if (entityId > maxEntityId) maxEntityId = entityId;
                            activeIdsSet.Add(entityId);
                        }
                    }
                }
            }

            _entitiesCounter = maxEntityId >= 0 ? maxEntityId + 1 : 0;
            _entities = new HashSet<VenusEntity>(_entitiesCounter);
            _freeEntities = new Queue<VenusEntity>(_entitiesCounter);
            _usedPools = new Dictionary<VenusEntity, HashSet<IVenusPool>>(_entitiesCounter);

            for (int id = 0; id < _entitiesCounter; id++)
            {
                var entity = new VenusEntity { Id = id };
                _usedPools.Add(entity, new HashSet<IVenusPool>(_pools.Count));
                if (activeIdsSet.Contains(id))
                {
                    _entities.Add(entity);
                }
                else
                {
                    _freeEntities.Enqueue(entity);
                }
            }

            // Rebuild _usedPools membership from pools' bitmasks
            for (int p = 0; p < _poolsArray.Length; p++)
            {
                var pool = _poolsArray[p];
                var bitmask = pool.Bitmask;
                int bitsPerInt = pool.BitsPerInt;
                int minId = pool.MinEntityId;
                for (int i = 0; i < bitmask.Length; i++)
                {
                    int mask = bitmask[i];
                    if (mask == 0) continue;
                    for (int b = 0; b < bitsPerInt; b++)
                    {
                        if ((mask & (1 << b)) != 0)
                        {
                            int entityId = minId + (i * bitsPerInt) + b;
                            if ((uint)entityId < (uint)_entitiesCounter)
                            {
                                var entity = new VenusEntity { Id = entityId };
                                _usedPools[entity].Add(pool);
                            }
                        }
                    }
                }
            }

            return;
        }
    }
}