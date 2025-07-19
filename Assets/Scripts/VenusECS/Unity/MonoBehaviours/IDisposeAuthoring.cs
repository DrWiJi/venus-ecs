using VenusECS.Core;

namespace VenusECS.Unity.MonoBehaviours
{
    public interface IDisposeAuthoring
    {
        void DisposeEntity(VenusEntity entity);
    }
}