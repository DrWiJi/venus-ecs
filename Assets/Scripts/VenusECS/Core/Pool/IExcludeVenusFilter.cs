using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IExcludeVenusFilter
    {
        List<IVenusPool> PoolsToExclude { get; }
    }
}