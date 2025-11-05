using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity.Extensions;

namespace Project.Core.Gameplay.Systems.Replication
{
    public class DirectReplicationComponentSystem<T> : IVenusSystem where T : struct, IVenusComponent
    {
        private VenusFilter _selfWorldFilter = new(new Include<T>());

        public void Tick()
        {
            foreach (var entity in _selfWorldFilter)
            {
                if(entity.HasSecondary<T>())
                    entity.Set<T>(entity.GetSecondary<T>());
                else
                    entity.Remove<T>();
            }
        }
    }
}