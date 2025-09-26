using System;
using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IVenusPool
    {
        event Action<IVenusPool, VenusEntity> OnEntityAdded;
        event Action<IVenusPool, VenusEntity> OnEntityRemoved;
        event Action<IVenusPool, VenusEntity, IVenusComponent> OnComponentChanged;
        T Get<T>(VenusEntity entity) where T : struct, IVenusComponent;
        T Add<T>(VenusEntity entity) where T : struct, IVenusComponent;
        void Set<T>(VenusEntity entity, T value) where T : struct, IVenusComponent;
        void Remove(VenusEntity entity);
        bool Has<T>(VenusEntity entity) where T : struct, IVenusComponent;
        unsafe void SetRaw(VenusEntity entity, byte* dataPtr);
        unsafe void AddRaw(VenusEntity entity, byte* dataPtr);
        void Clear();
        IEnumerable<VenusEntity> GetAllEntities();
        bool HasTyped(VenusEntity entity);
        Type GetComponentType();
        unsafe void* GetPointer(VenusEntity entity);
        int GetPoolIndex();
        byte[] GetSnapshot();
        void RestoreSnapshot(byte[] snapshot);

        // Bitmask properties
        int[] Bitmask { get; }
        int MinEntityId { get; }
        int MaxEntityId { get; }
        int BitsPerInt { get; }
    }
}