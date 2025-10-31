using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public abstract class BaseVenusIncludeFilter : IIncludeVenusFilter
    {
        protected IVenusPools _pools;
        private bool _useSecondaryPools;
        protected List<IVenusPools> _secondaryPools;

        public bool UseSecondaryPools => _useSecondaryPools;
        public abstract List<int> PoolsToInclude { get; }

        public IVenusPools Pools => _pools;

        public List<IVenusPools> SecondaryPools => _secondaryPools;

        public BaseVenusIncludeFilter(bool useSecondaryPools = false)
        {
            _pools = Venus.Pools;
            _secondaryPools = Venus.SecondaryPools;
            _useSecondaryPools = useSecondaryPools;
        }
    }
}