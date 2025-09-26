using UnityEngine;
using VenusECS.Core;

namespace VenusECS.Unity.Components
{
    public struct PositionComponent : IVenusComponent
    {
        public Vector2 Position;

        public bool Equals(IVenusComponent other)
        {
            return Position == ((PositionComponent)other).Position;
        }
    }
} 