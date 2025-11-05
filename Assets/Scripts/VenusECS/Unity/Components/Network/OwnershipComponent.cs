using MessagePack;
using VenusECS.Core;
namespace VenusECS.Unity.Components.Network
{
    [MessagePackObject]
    public struct OwnershipComponent : IVenusComponent
    {
        [Key(0)]
        public int OwnerId;

        public bool Equals(IVenusComponent other)
        {
            return OwnerId == ((OwnershipComponent)other).OwnerId;
        }
    }
}