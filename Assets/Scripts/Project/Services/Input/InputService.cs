using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Project.Services.Input
{  
    [CreateAssetMenu(fileName = "InputServiceAsset", menuName = "Project/Services/Input/InputServiceAsset")]
    public class InputService : ScriptableObject
    {
        [SerializeField]
        public InputActionAsset InputActions;
        
        public InputActionMap PlayerActionMap {get; private set;}
        public InputAction Move {get; private set;}
        public InputAction Look {get; private set;}
        
        [Inject]
        public void Construct()
        {
            PlayerActionMap = InputActions.FindActionMap("Player");
            Move = PlayerActionMap.FindAction("Move");
            Look = PlayerActionMap.FindAction("Look");
        }
    }
}
