using System;
using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IVenusPools
    {
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