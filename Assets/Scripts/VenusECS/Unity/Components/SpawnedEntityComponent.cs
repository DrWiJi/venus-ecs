using VenusECS.Core;

namespace VenusECS.Unity.Components
{
    public struct SpawnedEntityComponent : IVenusComponent
    {
        public bool Equals(IVenusComponent other)
        {
            return true;
        }
    }
}