using MessagePack;
using UnityEngine;
using VenusECS.Core;

namespace VenusECS.Unity.Components.Movement
{
    [MessagePackObject]
    public struct InputRecieverComponent : IVenusComponent
    {
        [Key(0)]
        public Vector2 Move;
        [Key(1)]
        public Vector2 Look;

        public bool Equals(IVenusComponent other)
        {
            return Move == ((InputRecieverComponent)other).Move && Look == ((InputRecieverComponent)other).Look;
        }
    }
}