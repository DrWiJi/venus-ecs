using VenusECS.Core;
using VenusECS.Core.Reflection.Attributes;

namespace VenusECS.Unity.Components
{
    [PoolSize(40000)]
    public struct TestComponent : IVenusComponent
    {
        public int SomeData;

        public bool Equals(IVenusComponent other)
        {
            return SomeData == ((TestComponent)other).SomeData;
        }
    }
}