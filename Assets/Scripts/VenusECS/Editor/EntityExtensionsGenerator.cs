using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using VenusECS.Core;

namespace VenusECS.CodeGeneration
{
    public static class EntityExtensionsGenerator
    {
        private static string _componentTypeTarget = "^componentType^";
        private static string _methodsTarget = "^methods^";
        private static string _additionalNamespacesTarget = "^additionalNamespace^";

        private static string _extensionsCodeTemplate = $@"//Generated automatically, dont touch with hands!
using System;
using VenusECS.Core;
using VenusECS.Core.Pool;
using VenusECS.Unity;
{_additionalNamespacesTarget}

namespace VenusECS.Extensions
{{
    public static class VenusEntityExtensions
    {{
{_methodsTarget}
    }}
}}
";

        private static string _addMethodTemplate =
            $@"        public static {_componentTypeTarget} Add{_componentTypeTarget}(this VenusEntity entity)
        {{
            var pool = ((VenusPools)Venus.Pools).{_componentTypeTarget}Pool;
            return pool.AddTyped(entity);
        }}";

        private static string _delMethodTemplate =
            $@"        public static void Del{_componentTypeTarget}(this VenusEntity entity)
        {{
            var pool = ((VenusPools)Venus.Pools).{_componentTypeTarget}Pool;
            pool.RemoveTyped(entity);
        }}";

        private static string _delIfExistsMethodTemplate =
            $@"        public static void DelIfExists{_componentTypeTarget}(this VenusEntity entity)
        {{
            var pool = ((VenusPools)Venus.Pools).{_componentTypeTarget}Pool;
            if (pool.HasTyped(entity))
            {{
                pool.RemoveTyped(entity);
            }}
        }}";

        private static string _getMethodTemplate =
            $@"        public static {_componentTypeTarget} Get{_componentTypeTarget}(this VenusEntity entity)
        {{
            var pool = ((VenusPools)Venus.Pools).{_componentTypeTarget}Pool;
            return pool.GetTyped(entity);
        }}
        
        public static {_componentTypeTarget} GetSecondary{_componentTypeTarget}(this VenusEntity entity)
        {{
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).{_componentTypeTarget}Pool.GetTyped(entity);
        }}";

        private static string _getOrAddMethodTemplate =
            $@"        public static {_componentTypeTarget} GetOrAdd{_componentTypeTarget}(this VenusEntity entity)
        {{
            var pool = ((VenusPools)Venus.Pools).{_componentTypeTarget}Pool;
            if (pool.HasTyped(entity))
            {{
                return pool.GetTyped(entity);
            }}
            return pool.AddTyped(entity);
        }}";

        private static string _hasMethodTemplate =
            $@"        public static bool Has{_componentTypeTarget}(this VenusEntity entity)
        {{
            var pool = ((VenusPools)Venus.Pools).{_componentTypeTarget}Pool;
            return pool.HasTyped(entity);
        }}
        
        public static bool HasSecondary{_componentTypeTarget}(this VenusEntity entity)
        {{
            if(!Venus.PrimaryWorldEntityToSecondaryWorldEntity[Venus.CurrentSecondaryPoolIndex].ContainsKey(entity))
            {{
                return false;
            }}
            return ((VenusPools)Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex]).{_componentTypeTarget}Pool.HasTyped(entity);
        }}";
        
        private static string _setMethodTemplate =
            $@"        public static void Set{_componentTypeTarget}(this VenusEntity entity, {_componentTypeTarget} component)
        {{
            var pool = ((VenusPools)Venus.Pools).{_componentTypeTarget}Pool;
            pool.SetTyped(entity, component);
        }}
        
        public static {_componentTypeTarget} Add{_componentTypeTarget}(this VenusEntity entity, {_componentTypeTarget} component)
        {{
            var pool = ((VenusPools)Venus.Pools).{_componentTypeTarget}Pool;
            pool.AddTyped(entity);
            pool.SetTyped(entity, component);
            return component;
        }}";

        private static string _generationTargetDirectory = "Assets/Scripts/VenusECS/Unity/Extensions/Generated";
        private static string _generationTargetFile = "VenusEntityExtensions.Methods.cs";
        private static string _stringEnd = "\r\n\r\n";

        [MenuItem("Tools/VenusECS/Generate Entity Extensions")]
        public static void GenerateCode()
        {
            PoolsGenerator.GenerateCode();
            var componentsTypesList = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var componentsTypes = assembly.GetTypes()
                    .Where(t => !t.IsClass && t.GetInterfaces().Contains(typeof(IVenusComponent)));
                foreach (var componentType in componentsTypes)
                {
                    componentsTypesList.Add(componentType);
                }
            }

            var namespacesHashset = new HashSet<string>();
            var componentsNamesList = new List<string>();
            foreach (var componentType in componentsTypesList)
            {
                var typeNamespace = componentType.Namespace;
                if (!string.IsNullOrEmpty(typeNamespace))
                {
                    if (!namespacesHashset.Contains(typeNamespace))
                    {
                        namespacesHashset.Add(typeNamespace);
                    }

                    componentsNamesList.Add(componentType.Name);
                }
            }

            var namespacesStringBuilder = new StringBuilder();
            foreach (var typeNamespace in namespacesHashset)
            {
                namespacesStringBuilder.Append("using ");
                namespacesStringBuilder.Append(typeNamespace);
                namespacesStringBuilder.Append(";\r\n");
            }

            var methodsBuilder = new StringBuilder();
            foreach (var componentType in componentsNamesList)
            {
                // Add method
                methodsBuilder.Append(_addMethodTemplate.Replace(_componentTypeTarget, componentType));
                methodsBuilder.Append(_stringEnd);
                
                // Del method
                methodsBuilder.Append(_delMethodTemplate.Replace(_componentTypeTarget, componentType));
                methodsBuilder.Append(_stringEnd);
                
                // DelIfExists method
                methodsBuilder.Append(_delIfExistsMethodTemplate.Replace(_componentTypeTarget, componentType));
                methodsBuilder.Append(_stringEnd);
                
                // Get method
                methodsBuilder.Append(_getMethodTemplate.Replace(_componentTypeTarget, componentType));
                methodsBuilder.Append(_stringEnd);
                
                // GetOrAdd method
                methodsBuilder.Append(_getOrAddMethodTemplate.Replace(_componentTypeTarget, componentType));
                methodsBuilder.Append(_stringEnd);
                
                // Has method
                methodsBuilder.Append(_hasMethodTemplate.Replace(_componentTypeTarget, componentType));
                methodsBuilder.Append(_stringEnd);
                
                // Set method
                methodsBuilder.Append(_setMethodTemplate.Replace(_componentTypeTarget, componentType));
                methodsBuilder.Append(_stringEnd);
            }

            var generatedExtensions = new StringBuilder();
            generatedExtensions.Append(_extensionsCodeTemplate);
            generatedExtensions.Replace(_additionalNamespacesTarget, namespacesStringBuilder.ToString());
            generatedExtensions.Replace(_methodsTarget, methodsBuilder.ToString());
            
            // Ensure directory exists
            Directory.CreateDirectory(_generationTargetDirectory);
            
            File.WriteAllText(Path.Combine(_generationTargetDirectory, _generationTargetFile), generatedExtensions.ToString());
            
            AssetDatabase.Refresh();
        }
    }
} 