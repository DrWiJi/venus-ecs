using VenusECS.Core;
using VenusECS.Core.Reflection.Attributes;

namespace VenusECS.Unity.Components
{
    [PoolSize(40000)]
    public struct OtherTestComponent : IVenusComponent
    {
        public int OtherData;

        public bool Equals(IVenusComponent other)
        {
            return OtherData == ((OtherTestComponent)other).OtherData;
        }
    }
}