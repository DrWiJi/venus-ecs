using System;
using System.Linq;
using System.Reflection;
using VenusECS.Core.Pool;

namespace VenusECS.Core
{
    public static class Venus
    {
        private static IVenusPools _pools;

        public static IVenusPools Pools => _pools;

        static Venus()
        {
            InitializePools();
        }

        private static void InitializePools()
        {
            var factoryType = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => !type.IsAbstract && !type.IsInterface)
                .FirstOrDefault(type => typeof(IVenusPoolsFactory).IsAssignableFrom(type));

            if (factoryType == null)
            {
                throw new InvalidOperationException("No implementation of IVenusPoolsFactory found in current domain");
            }

            var factory = (IVenusPoolsFactory)Activator.CreateInstance(factoryType);
            _pools = factory.Create();
        }

        public static void Reset()
        {
            _pools.Clear();
        }
    }
}