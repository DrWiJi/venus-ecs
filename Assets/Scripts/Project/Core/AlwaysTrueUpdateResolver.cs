using VenusECS.Core;

namespace Project.Core
{
    public class AlwaysTrueUpdateResolver : VenusModule.IVenusModuleUpdateResolver
    {
        public bool Check()
        {
            return true;
        }
    }
}