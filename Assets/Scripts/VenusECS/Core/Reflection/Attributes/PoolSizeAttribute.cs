using System;

namespace VenusECS.Core.Reflection.Attributes
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class PoolSizeAttribute : Attribute
    {
        public int PoolSize { get; private set; }
        public PoolSizeAttribute(int size)
        {
            PoolSize = size;
        }
    }
}