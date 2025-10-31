using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IExcludeVenusFilter
    {
        IVenusPools Pools { get; }
        List<IVenusPools> SecondaryPools { get; }
        bool UseSecondaryPools { get; }
        List<int> PoolsToExclude { get; }
    }
}