using VenusECS.Core;
using VenusECS.Core.Pool;

namespace Project.Core.Gameplay.Systems.Replication
{
    public class SecondaryWorldsEntitiesCleanerSystem : IVenusSystem
    {
        private void Tick()
        {
            IVenusPools secondaryPools = Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex];
            foreach (var entity in secondaryPools.Entities)
            {
                if (secondaryPools.GetEntityComponentsCount(entity) == 0)
                {
                    secondaryPools.DeleteEntity(entity);
                    var primaryWorldEntity = Venus.SecondaryWorldEntityToPrimaryWorldEntity[Venus.CurrentSecondaryPoolIndex][entity];
                    Venus.SecondaryWorldEntityToPrimaryWorldEntity[Venus.CurrentSecondaryPoolIndex].Remove(entity);
                    Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].Remove(primaryWorldEntity);
                }
            }
        }
    }
}