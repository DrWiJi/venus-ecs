using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public interface IIncludeVenusFilter
    {
        IVenusPools Pools { get; }
        List<IVenusPools> SecondaryPools { get; }
        bool UseSecondaryPools { get; }
        List<int> PoolsToInclude { get; }
    }
}