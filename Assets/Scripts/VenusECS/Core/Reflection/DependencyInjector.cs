using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VenusECS.Core.Reflection.Attributes;

namespace VenusECS.Core.Reflection
{
    public class DependencyInjector
    {
        private HashSet<Type> _autoInstantiate = new();
        private Dictionary<Type, object> _services = new();
        private Dictionary<Type, object> _dynamicInstances = new();
        
        public void AutoSharedInstantiateType(Type type)
        {
            _autoInstantiate.Add(type);
        }
        
        public void AddService(Type type, object instance)
        {
            _services.Add(type, instance);
        }

        public void InjectDependencies(object target)
        {
            var targetType = target.GetType();
            var diFields = targetType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Select(f => f.GetCustomAttributes(typeof(VenusInjectAttribute), true).Length > 0 ? f : null).ToList();
            foreach (var field in diFields)
            {
                if (field == null) continue;
                var fieldType = field.FieldType;
                if (_services.ContainsKey(fieldType))
                {
                    field.SetValue(target, _services[fieldType]);
                }
                else if (_autoInstantiate.Contains(fieldType))
                {
                    if (!_dynamicInstances.ContainsKey(fieldType))
                    {
                        _dynamicInstances.Add(fieldType, Activator.CreateInstance(fieldType));
                    }
                    
                    field.SetValue(target, _dynamicInstances[fieldType]);
                }
            }
        }
    }
}