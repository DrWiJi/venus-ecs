using VenusECS.Core;
namespace VenusECS.Unity.Components.Network
{
    public struct OwnershipRequestComponent : IVenusComponent
    {
        public bool Equals(IVenusComponent other)
        {
            return true;
        }
    }
}