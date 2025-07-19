using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using VenusECS.Core;

namespace VenusECS.CodeGeneration
{
    public static class PoolsGenerator
    {
        private static string _componentTypeTarget = "^componentType^";
        private static string _poolNameTarget = "^poolName^";
        private static string _poolCreationTarget = "^pool^";
        private static string _poolAddTarget = "^poolAdd^";
        private static string _additionalNamespacesTarget = "^additionalNamespace^";

        private static string _poolsCodeTemplate = $@"//Generated automatically, dont touch with hands!
using System;
using System.Collections.Generic;
using VenusECS.Core.Pool;
using VenusECS.Core;
{_additionalNamespacesTarget}

namespace VenusECS.Unity
{{
    public partial class VenusPools
    {{
{_poolCreationTarget}

        private void AddGenerated()
        {{
{_poolAddTarget}
        }}
    }}
}}
";

        private static string _poolFieldTemplate =
            $@"        public readonly VenusPool<{_componentTypeTarget}> {_poolNameTarget} = new VenusPool<{_componentTypeTarget}>()";

        private static string _poolAddTemplate =
            $@"            _pools.Add(typeof({_componentTypeTarget}), {_poolNameTarget})";

        private static string _generationTargetDirectory = "Assets/Scripts/VenusECS/Unity/Generated/Pools";
        private static string _generationTargetFile = "VenusPools.Generated.cs";
        private static string _stringEnd = ";\r\n";

        [MenuItem("Tools/VenusECS/Generate Pools Code")]
        public static void GenerateCode()
        {
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
                namespacesStringBuilder.Append(_stringEnd);
            }

            var componentsDefinitions = new StringBuilder();
            var componentsInstantiations = new StringBuilder();
            foreach (var componentType in componentsNamesList)
            {
                var poolName = componentType + "Pool";
                componentsDefinitions.Append(_poolFieldTemplate.Replace(_componentTypeTarget, componentType).Replace(_poolNameTarget, poolName));
                componentsDefinitions.Append(_stringEnd);
                componentsInstantiations.Append(_poolAddTemplate.Replace(_componentTypeTarget, componentType).Replace(_poolNameTarget, poolName));
                componentsInstantiations.Append(_stringEnd);
            }

            var generatedPool = new StringBuilder();
            generatedPool.Append(_poolsCodeTemplate);
            generatedPool.Replace(_additionalNamespacesTarget, namespacesStringBuilder.ToString());
            generatedPool.Replace(_poolCreationTarget, componentsDefinitions.ToString());
            generatedPool.Replace(_poolAddTarget, componentsInstantiations.ToString());
            File.WriteAllText(Path.Combine(_generationTargetDirectory, _generationTargetFile),generatedPool.ToString());
        }
    }
}