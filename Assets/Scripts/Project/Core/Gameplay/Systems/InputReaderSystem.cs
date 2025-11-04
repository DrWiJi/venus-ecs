using Project.Services.Input;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Core.Reflection.Attributes;
using VenusECS.Unity.Components.Movement;

namespace Project.Core.Gameplay.Systems
{
    public class InputReaderSystem : IVenusSystem
    {
        [VenusInject]
        private InputService _inputService;

        private VenusFilter _inputRecieverEntityFilter = new(new Include<InputRecieverComponent>());
        
        public void Tick()
        {

        }
    }
}