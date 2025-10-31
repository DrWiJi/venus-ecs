using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public abstract class BaseVenusExcludeFilter : IExcludeVenusFilter
    {
        private bool _useSecondaryPools;
        protected IVenusPools _pools;
        protected List<IVenusPools> _secondaryPools;

        public bool UseSecondaryPools => _useSecondaryPools;
        public abstract List<int> PoolsToExclude { get; }

        public IVenusPools Pools => _pools;

        public List<IVenusPools> SecondaryPools => _secondaryPools;

        public BaseVenusExcludeFilter(bool useSecondaryPools = false)
        {
            _pools = Venus.Pools;
            _secondaryPools = Venus.SecondaryPools;
            _useSecondaryPools = useSecondaryPools;
        }
    }
}