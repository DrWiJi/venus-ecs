using VenusECS.Core;
using VenusECS.Core.Pool;

namespace VenusECS.Unity
{
    public partial class VenusPools : VenusPoolsBase
    {        
        public VenusPools() : base(1024)
        {
            AddGenerated();
        }

        public override IVenusPool GetPool<T>()
        {
            return _pools[typeof(T)];
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}