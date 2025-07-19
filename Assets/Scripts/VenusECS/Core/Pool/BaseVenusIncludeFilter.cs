using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public abstract class BaseVenusIncludeFilter : IIncludeVenusFilter
    {
        protected IVenusPools _pools;
        public abstract List<IVenusPool> PoolsToInclude { get; }

        public BaseVenusIncludeFilter()
        {
            _pools = Venus.Pools;
        }
    }
}