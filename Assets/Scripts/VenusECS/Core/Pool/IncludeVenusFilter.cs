using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public class Include<T1> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent
    {
        private List<int> _pool;
        public override List<int> PoolsToInclude => _pool;

        public Include(bool useSecondaryPools = false) : base(useSecondaryPools)
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>().GetPoolIndex());
        }
    }

    public class Include<T1, T2> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent 
        where T2 : struct, IVenusComponent
    {
        private List<int> _pool;
        public override List<int> PoolsToInclude => _pool;

        public Include(bool useSecondaryPools = false) : base(useSecondaryPools)
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T2>().GetPoolIndex());
        }
    }

    public class Include<T1, T2, T3> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent 
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
    {
        private List<int> _pool;
        public override List<int> PoolsToInclude => _pool;

        public Include(bool useSecondaryPools = false) : base(useSecondaryPools)
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T2>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T3>().GetPoolIndex());
        }
    }

    public class Include<T1, T2, T3, T4> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent 
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
    {
        private List<int> _pool;
        public override List<int> PoolsToInclude => _pool;

        public Include(bool useSecondaryPools = false) : base(useSecondaryPools)
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T2>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T3>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T4>().GetPoolIndex());
        }
    }

    public class Include<T1, T2, T3, T4, T5> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent 
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
        where T5 : struct, IVenusComponent
    {
        private List<int> _pool;
        public override List<int> PoolsToInclude => _pool;

        public Include(bool useSecondaryPools = false) : base(useSecondaryPools)
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T2>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T3>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T4>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T5>().GetPoolIndex());
        }
    }

    public class Include<T1, T2, T3, T4, T5, T6> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent 
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
        where T5 : struct, IVenusComponent
        where T6 : struct, IVenusComponent
    {
        private List<int> _pool;
        public override List<int> PoolsToInclude => _pool;
        public Include(bool useSecondaryPools = false) : base(useSecondaryPools)
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T2>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T3>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T4>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T5>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T6>().GetPoolIndex());
        }
    }

    public class Include<T1, T2, T3, T4, T5, T6, T7> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent 
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
        where T5 : struct, IVenusComponent
        where T6 : struct, IVenusComponent
        where T7 : struct, IVenusComponent
    {
        private List<int> _pool;

        public override List<int> PoolsToInclude => _pool;

        public Include(bool useSecondaryPools = false) : base(useSecondaryPools)
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T2>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T3>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T4>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T5>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T6>().GetPoolIndex());
            _pool.Add(_pools.GetPool<T7>().GetPoolIndex());
        }
    }

}