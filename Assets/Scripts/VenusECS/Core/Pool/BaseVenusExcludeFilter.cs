using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public abstract class BaseVenusExcludeFilter : IExcludeVenusFilter
    {
        protected IVenusPools _pools;
        public abstract List<IVenusPool> PoolsToExclude { get; }

        public BaseVenusExcludeFilter()
        {
            _pools = Venus.Pools;
        }
    }
}