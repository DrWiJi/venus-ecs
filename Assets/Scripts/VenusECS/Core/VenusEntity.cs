using System;
using MessagePack;

namespace VenusECS.Core
{
    [MessagePackObject]
    public struct VenusEntity : IEquatable<VenusEntity>
    {
        [Key(0)]
        public int Id;

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public bool Equals(VenusEntity other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is VenusEntity other && Equals(other);
        }
    }
}