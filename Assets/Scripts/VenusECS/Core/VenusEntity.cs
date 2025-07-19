using System;

namespace VenusECS.Core
{
    public struct VenusEntity : IEquatable<VenusEntity>
    {
        public int Id;
        public int Generation;

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Generation);
        }

        public bool Equals(VenusEntity other)
        {
            return Id == other.Id && Generation == other.Generation;
        }

        public override bool Equals(object obj)
        {
            return obj is VenusEntity other && Equals(other);
        }
    }
}