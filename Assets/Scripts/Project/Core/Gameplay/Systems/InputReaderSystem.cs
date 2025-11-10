using Project.Services.Input;
using UnityEngine;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Core.Reflection.Attributes;
using VenusECS.Extensions;
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
            foreach (var entity in _inputRecieverEntityFilter)
            {
                var inputReciever = entity.GetInputRecieverComponent();
                inputReciever.Move = _inputService.Move.ReadValue<Vector2>();
                inputReciever.Look = _inputService.Look.ReadValue<Vector2>();
                entity.SetInputRecieverComponent(inputReciever);
            }
        }
    }
}