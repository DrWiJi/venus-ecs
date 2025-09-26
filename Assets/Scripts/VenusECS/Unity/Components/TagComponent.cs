using VenusECS.Core;

namespace VenusECS.Unity.Components
{
    public struct TagComponent : IVenusComponent
    {
        public int Tag;

        public bool Equals(IVenusComponent other)
        {
            return Tag == ((TagComponent)other).Tag;
        }
    }
} 