using System;
using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IVenusPool
    {
        event Action<IVenusPool, VenusEntity> OnEntityAdded;
        event Action<IVenusPool, VenusEntity> OnEntityRemoved;
        T Get<T>(VenusEntity entity) where T : struct, IVenusComponent;
        T Add<T>(VenusEntity entity) where T : struct, IVenusComponent;
        void Set<T>(VenusEntity entity, T value) where T : struct, IVenusComponent;
        void Remove(VenusEntity entity);
        bool Has<T>(VenusEntity entity) where T : struct, IVenusComponent;
        void Clear();
        IEnumerable<VenusEntity> GetAllEntities();
        bool HasTyped(VenusEntity entity);
        
        // Bitmask properties
        int[] Bitmask { get; }
        int MinEntityId { get; }
        int MaxEntityId { get; }
        int BitsPerInt { get; }
    }
}