using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VenusECS.Core.Pool;

namespace VenusECS.Core
{
    public static class Venus
    {
        private static IVenusPools _pools;
        private static IVenusPoolsFactory _poolsFactory;

        public static IVenusPools Pools => _pools;
        
        public static List<IVenusPools> SecondaryPools = new();

        static Venus()
        {
            InitializeMainPools();
        }

        private static void InitializeMainPools()
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
            _poolsFactory = factory;
        }

        public static void CreateSecondaryPools()
        {
            SecondaryPools.Add(_poolsFactory.Create());
        }

        public static void Reset()
        {
            _pools.Clear();
            foreach (var pool in SecondaryPools)
            {
                pool.Clear();
            }
            SecondaryPools.Clear();
        }
    }
}