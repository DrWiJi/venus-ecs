using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Project.Services.Input
{  
    [CreateAssetMenu(fileName = "InputServiceAsset", menuName = "Project/Services/Input/InputServiceAsset")]
    public class InputService : ScriptableObject
    {
        [SerializeField]
        public InputActionAsset _inputActions;
        
        [Inject]
        public void Construct()
        {

        }
    }
}
