using System;
using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IVenusPools
    {
        int WorldIndex { get; set;}
        event Action<VenusEntity> OnEntityCreated;
        event Action<VenusEntity> OnEntityDeleted;
        event Action<int, VenusEntity> OnEntityCreatedExternally;
        event Action<int, VenusEntity> OnEntityDeletedExternally;
        IEnumerable<VenusEntity> Entities { get; }
        int GetEntityComponentsCount(VenusEntity entity);
        IVenusPool GetPool<T1>() where T1 : struct, IVenusComponent;
        IVenusPool GetPool(int index);
        VenusEntity CreateEntity();
        void ApplyDelta(PoolsDeltaPortion[] deltaPortions, byte[] deltaPortionsData);
        (PoolsDeltaPortion[] deltaPortions, byte[] deltaPortionsData) FlushDelta();
        byte[] GetSnapshot();
        void RestoreSnapshot(byte[] snapshot);
        void DeleteEntity(VenusEntity venusEntity);
        void Clear();
    }
}