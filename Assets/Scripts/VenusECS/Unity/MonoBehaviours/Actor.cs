using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VenusECS.Core;

namespace VenusECS.Unity.MonoBehaviours
{
    public class Actor : MonoBehaviour
    {
        private List<IInitAuthoring> _initAuthoring = new();
        private List<IDisposeAuthoring> _disposeAuthoring = new();
     
        public VenusEntity Entity { get; private set; }
        
        private void Start()
        {
            _initAuthoring = GetComponents<IInitAuthoring>().ToList();
            _disposeAuthoring = GetComponents<IDisposeAuthoring>().ToList();

            var entity = Venus.Pools.CreateEntity();
            Entity = entity;
            
            foreach (var initAuthoring in _initAuthoring)
            {
                initAuthoring.InitEntity(entity);
            }
        }

        private void OnDestroy()
        {
            foreach (var disposeAuthoring in _disposeAuthoring)
            {
                disposeAuthoring.DisposeEntity(Entity);
            }
        }
    }
}