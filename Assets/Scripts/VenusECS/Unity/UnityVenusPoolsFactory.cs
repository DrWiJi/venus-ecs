using VenusECS.Core;
using VenusECS.Core.Pool;

namespace VenusECS.Unity
{
    public class UnityVenusPoolsFactory : IVenusPoolsFactory
    {
        public IVenusPools Create()
        {
            return new VenusPools();
        }
    }
}