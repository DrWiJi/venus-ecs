using System;
using VenusECS.Core;

namespace VenusECS.Unity.Extensions
{
    public static partial class VenusEntityExtensions
    {
        public static T Get<T>(this VenusEntity entity) where T: struct, IVenusComponent
        {
            return Venus.Pools.GetPool<T>().Get<T>(entity);
        }

        public static T GetSecondary<T>(this VenusEntity entity) where T: struct, IVenusComponent
        {
            return Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex].GetPool<T>().Get<T>(entity);
        }

        public static void Set<T>(this VenusEntity entity, T component) where T: struct, IVenusComponent
        {            
            if (!entity.Has<T>())
            {
                entity.Add<T>();
            }
            Venus.Pools.GetPool<T>().Set<T>(entity, component);
        }
        
        public static T Add<T>(this VenusEntity entity) where T: struct, IVenusComponent
        {
            return Venus.Pools.GetPool<T>().Add<T>(entity);
        }
        
        public static T GetOrAdd<T>(this VenusEntity entity) where T: struct, IVenusComponent
        {
            if (!entity.Has<T>())
            {
                return entity.Add<T>();
            }
            return Venus.Pools.GetPool<T>().Get<T>(entity);
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

        public static bool HasSecondary<T>(this VenusEntity entity) where T : struct, IVenusComponent
        {
            return Venus.SecondaryPools[Venus.CurrentSecondaryPoolIndex].GetPool<T>().Has<T>(entity);
        }

        public static void Remove<T>(this VenusEntity entity) where T : struct, IVenusComponent
        {
            Venus.Pools.GetPool<T>().Remove(entity);
        }
    }
}