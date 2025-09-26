using System;
using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IVenusPools
    {
        IVenusPool GetPool<T1>() where T1 : struct, IVenusComponent;
        VenusEntity CreateEntity();
        void ApplyDelta(PoolsDeltaPortion[] deltaPortions, byte[] deltaPortionsData);
        (PoolsDeltaPortion[], byte[]) FlushDelta();
        byte[] GetSnapshot();
        void RestoreSnapshot(byte[] snapshot);
        void DeleteEntity(VenusEntity venusEntity);
        void Clear();
    }
}