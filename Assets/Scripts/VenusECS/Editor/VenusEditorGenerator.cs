using UnityEditor;

namespace VenusECS.CodeGeneration
{
    public static class VenusEditorGenerator
    {
        [MenuItem("Tools/VenusECS/Generate Code", priority = 1000)]
        public static void GenerateAllCode()
        {
            EntityExtensionsGenerator.GenerateCode();
            PoolsGenerator.GenerateCode();
        }
    }
}