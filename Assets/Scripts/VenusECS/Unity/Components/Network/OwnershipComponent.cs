using VenusECS.Core;
namespace VenusECS.Unity.Components.Network
{
    public struct OwnershipComponent : IVenusComponent
    {
        public int OwnerId;

        public bool Equals(IVenusComponent other)
        {
            return OwnerId == ((OwnershipComponent)other).OwnerId;
        }
    }
}