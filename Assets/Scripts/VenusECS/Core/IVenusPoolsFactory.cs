using VenusECS.Core.Pool;

namespace VenusECS.Core
{
    public interface IVenusPoolsFactory
    {
        IVenusPools Create();
    }
}