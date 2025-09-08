using System;

namespace VenusECS.Core
{
    public struct VenusEntity : IEquatable<VenusEntity>
    {
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