using System;
using VenusECS.Core;

namespace VenusECS.Unity.Extensions
{
    public static partial class VenusEntityExtensions
    {
        public static ref T Get<T>(this VenusEntity entity) where T: struct, IVenusComponent
        {
            return ref Venus.Pools.GetPool<T>().Get<T>(entity);
        }
        
        public static ref T Add<T>(this VenusEntity entity) where T: struct, IVenusComponent
        {
            return ref Venus.Pools.GetPool<T>().Add<T>(entity);
        }
        
        public static ref T GetOrAdd<T>(this VenusEntity entity) where T: struct, IVenusComponent
        {
            if (!entity.Has<T>())
            {
                return ref entity.Add<T>();
            }
            return ref Venus.Pools.GetPool<T>().Get<T>(entity);
        }

        public static void DelIfExists<T>(this VenusEntity entity) where T: struct, IVenusComponent
        {
            if (entity.Has<T>())
            {
                entity.Remove<T>();
            }
        }

        public static bool Has<T>(this VenusEntity entity) where T : struct, IVenusComponent
        {
            return Venus.Pools.GetPool<T>().Has<T>(entity);
        }

        public static void Remove<T>(this VenusEntity entity) where T : struct, IVenusComponent
        {
            Venus.Pools.GetPool<T>().Remove(entity);
        }
    }
}