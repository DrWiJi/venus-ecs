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
        
        public static readonly List<IVenusPools> SecondaryPools = new();

        public static int CurrentSecondaryPoolIndex = 0;

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

        public static int CreateSecondaryPools()
        {
            SecondaryPools.Add(_poolsFactory.Create());
            return SecondaryPools.Count - 1;
        }

        public static void RemoveSecondaryPool(int index)
        {
            SecondaryPools.RemoveAt(index);
        }

        public static void SwitchSecondaryPool(int index)
        {
            CurrentSecondaryPoolIndex = index;
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