using System;
using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IVenusPools
    {
        IVenusPool GetPool<T1>() where T1 : struct, IVenusComponent;
        VenusEntity CreateEntity();
        void DeleteEntity(VenusEntity venusEntity);
        void Clear();
    }
}