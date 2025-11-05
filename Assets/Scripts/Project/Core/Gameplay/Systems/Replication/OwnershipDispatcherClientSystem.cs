using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity.Components.Network;
using VenusECS.Extensions;
using VenusECS.Unity.Extensions;

namespace Project.Core.Gameplay.Systems.Replication
{
    public class OwnershipDispatcherClientSystem : IVenusSystem
    {
        private VenusFilter _ownershipRequestFilter = new(new Include<OwnershipRequestComponent>());

        public void Tick()
        {
            foreach (var entity in _ownershipRequestFilter)
            {
                if(entity.HasSecondary<OwnershipComponent>())
                {
                    var ownershipComponent = entity.GetSecondary<OwnershipComponent>();
                    entity.SetOwnershipComponent(ownershipComponent);
                    entity.DelOwnershipRequestComponent();
                }                
            }
        }
    }
}