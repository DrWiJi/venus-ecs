using VenusECS.Core;
using VenusECS.Unity.MonoBehaviours;

namespace VenusECS.Unity.Components
{
    public struct ActorReference : IVenusComponent
    {
        public Actor Actor;
    }
}