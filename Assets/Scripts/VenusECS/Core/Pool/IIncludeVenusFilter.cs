using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IIncludeVenusFilter
    {
        List<IVenusPool> PoolsToInclude { get; }
    }
}