using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public class Include<T1> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToInclude => _pool;

        public Include() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
        }
    }

    public class Include<T1, T2> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent 
        where T2 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToInclude => _pool;

        public Include() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
        }
    }

    public class Include<T1, T2, T3> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToInclude => _pool;

        public Include() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
        }
    }

    public class Include<T1, T2, T3, T4> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToInclude => _pool;

        public Include() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
            _pool.Add(_pools.GetPool<T4>());
        }
    }

    public class Include<T1, T2, T3, T4, T5> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
        where T5 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToInclude => _pool;

        public Include() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
            _pool.Add(_pools.GetPool<T4>());
            _pool.Add(_pools.GetPool<T5>());
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
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToInclude => _pool;

        public Include() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
            _pool.Add(_pools.GetPool<T4>());
            _pool.Add(_pools.GetPool<T5>());
            _pool.Add(_pools.GetPool<T6>());
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
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToInclude => _pool;

        public Include() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
            _pool.Add(_pools.GetPool<T4>());
            _pool.Add(_pools.GetPool<T5>());
            _pool.Add(_pools.GetPool<T6>());
            _pool.Add(_pools.GetPool<T7>());
        }
    }
    public class Include<T1, T2, T3, T4, T5, T6, T7, T8> : BaseVenusIncludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
        where T5 : struct, IVenusComponent
        where T6 : struct, IVenusComponent
        where T7 : struct, IVenusComponent
        where T8 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToInclude => _pool;

        public Include() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
            _pool.Add(_pools.GetPool<T4>());
            _pool.Add(_pools.GetPool<T5>());
            _pool.Add(_pools.GetPool<T6>());
            _pool.Add(_pools.GetPool<T7>());
            _pool.Add(_pools.GetPool<T8>());
        }
    }
}