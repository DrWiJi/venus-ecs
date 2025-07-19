using System.Collections.Generic;
using VenusECS.Core.Exceptions;

namespace VenusECS.Core
{
    public sealed class VenusModule
    {
        private IVenusModuleUpdateResolver _enableTickModuleCheck;

        private List<IVenusInitSystem> _initSystems = new();
        private List<IVenusTickSystem> _tickSystems = new();
        private List<IVenusDisposeSystem> _disposeSystems = new();
        
        private bool _inited;
        
        public interface IVenusModuleUpdateResolver
        {
            bool Check();
        }

        public static VenusModule Create(IVenusModuleUpdateResolver enableTickModuleCheck = null)
        {
            var module = new VenusModule(enableTickModuleCheck);
            return module;
        }

        public VenusModule(IVenusModuleUpdateResolver enableTickModuleCheck = null)
        {
            _enableTickModuleCheck = enableTickModuleCheck;
        }

        public VenusModule AddSystem(IVenusSystem system)
        {
            if (system is IVenusInitSystem initSystem)
            {
                _initSystems.Add(initSystem);
            }
            if (system is IVenusTickSystem tickSystem)
            {
                _tickSystems.Add(tickSystem);
            }

            if (system is IVenusDisposeSystem disposeSystem)
            {
                _disposeSystems.Add(disposeSystem);
            }

            return this;
        }
        
        public void Init()
        {
            if (_inited)
            {
                throw new CallOrderViolationException("Trying to init VenusModule twice.");
            }
            _inited = true;
            foreach (var system in _initSystems)
            {
                system.Init();
            }
        }

        public void Tick()
        {
            if (_enableTickModuleCheck == null || _enableTickModuleCheck.Check())
            {
                foreach (var system in _tickSystems)
                {
                    system.Tick();
                }
            }
        }

        public void Dispose()
        {            
            if (!_inited)
            {
                throw new CallOrderViolationException("Trying to dispose uninitialized VenusModule.");
            }
            _inited = false;
            foreach (var system in _disposeSystems)
            {
                system.Dispose();
            }
        }
    }
}