using VenusECS.Core;

namespace VenusECS.Unity.Components
{
    public struct VenusEntityComponent : IVenusComponent
    {
        public bool Equals(IVenusComponent other)
        {
            return true;
        }
    }
}