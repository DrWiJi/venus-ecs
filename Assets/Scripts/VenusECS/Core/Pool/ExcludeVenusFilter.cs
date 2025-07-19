using System.Collections.Generic;

namespace VenusECS.Core.Pool
{
    public class Exclude<T1> : BaseVenusExcludeFilter 
        where T1 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToExclude => _pool;

        public Exclude() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
        }
    }
    
    public class Exclude<T1,T2> : BaseVenusExcludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToExclude => _pool;

        public Exclude() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
        }
    }

    public class Exclude<T1,T2,T3> : BaseVenusExcludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToExclude => _pool;

        public Exclude() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
        }
    }

    public class Exclude<T1,T2,T3,T4> : BaseVenusExcludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToExclude => _pool;

        public Exclude() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
            _pool.Add(_pools.GetPool<T4>());
        }
    }

    public class Exclude<T1,T2,T3,T4,T5> : BaseVenusExcludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
        where T5 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToExclude => _pool;

        public Exclude() : base()
        {
            _pool = new();
            _pool.Add(_pools.GetPool<T1>());
            _pool.Add(_pools.GetPool<T2>());
            _pool.Add(_pools.GetPool<T3>());
            _pool.Add(_pools.GetPool<T4>());
            _pool.Add(_pools.GetPool<T5>());
        }
    }

    public class Exclude<T1,T2,T3,T4,T5,T6> : BaseVenusExcludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
        where T5 : struct, IVenusComponent
        where T6 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToExclude => _pool;

        public Exclude() : base()
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

    public class Exclude<T1,T2,T3,T4,T5,T6,T7> : BaseVenusExcludeFilter 
        where T1 : struct, IVenusComponent
        where T2 : struct, IVenusComponent
        where T3 : struct, IVenusComponent
        where T4 : struct, IVenusComponent
        where T5 : struct, IVenusComponent
        where T6 : struct, IVenusComponent
        where T7 : struct, IVenusComponent
    {
        private List<IVenusPool> _pool;

        public override List<IVenusPool> PoolsToExclude => _pool;

        public Exclude() : base()
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
}