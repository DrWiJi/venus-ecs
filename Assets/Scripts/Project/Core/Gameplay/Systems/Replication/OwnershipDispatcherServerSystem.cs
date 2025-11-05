using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity.Components.Network;
using VenusECS.Extensions;

namespace Project.Core.Gameplay.Systems.Replication
{
    public class OwnershipDispatcherServerSystem : IVenusSystem
    {
        private VenusFilter _ownershipRequestFilter = new(new Include<OwnershipRequestComponent>(true), new Exclude<OwnershipComponent>());

        public void Tick()
        {
            foreach (var entity in _ownershipRequestFilter)
            {
                var ownershipComponent = entity.AddOwnershipComponent();
                ownershipComponent.OwnerId = Venus.CurrentSecondaryPoolIndex;
                entity.SetOwnershipComponent(ownershipComponent);
            }
        }
    }
}