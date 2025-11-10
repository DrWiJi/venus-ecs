using VenusECS.Core;

namespace VenusECS.Unity.Components.Movement
{
    public struct LocalInputRecieverComponent : IVenusComponent
    {
        public bool Equals(IVenusComponent other)
        {
            return true;
        }
    }
}