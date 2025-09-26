using VenusECS.Core;

namespace VenusECS.Unity.Components
{
    public struct EmptyComponent : IVenusComponent
    {
        // Deliberately empty component for testing
        public bool Equals(IVenusComponent other)
        {
            return true;
        }
    }
} 