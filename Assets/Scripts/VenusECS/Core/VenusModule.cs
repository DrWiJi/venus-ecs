using System;
using System.Collections.Generic;
using VenusECS.Core.Exceptions;
using VenusECS.Core.Reflection;

namespace VenusECS.Core
{
    public sealed class VenusModule
    {
        private IVenusModuleUpdateResolver _enableTickModuleCheck;
        private DependencyInjector _dependencyInjector;
        private List<VenusModule> _modules = new();
        private List<IVenusInitSystem> _initSystems = new();
        private List<IVenusTickSystem> _tickSystems = new();
        private List<IVenusDisposeSystem> _disposeSystems = new();
        
        private bool _inited;
        
        public interface IVenusModuleUpdateResolver
        {
            bool Check();
        }

        public VenusModule(IVenusModuleUpdateResolver enableTickModuleCheck = null)
        {
            _enableTickModuleCheck = enableTickModuleCheck;
        }

        public void SetDependencyInjector(DependencyInjector dependencyInjector)
        {
            _dependencyInjector = dependencyInjector;
            // Propagate the same injector to submodules to share services
            foreach (var module in _modules)
            {
                module.SetDependencyInjector(_dependencyInjector);
            }
        }

        public void SetResolver(IVenusModuleUpdateResolver enableTickModuleCheck)
        {
            _enableTickModuleCheck = enableTickModuleCheck;
        }

        public VenusModule AddService(Type type, object instance)
        {
            if (_dependencyInjector == null)
            {
                _dependencyInjector = new DependencyInjector();
            }
            _dependencyInjector.AddService(type, instance);
            return this;
        }

        public VenusModule AddModule(VenusModule module)
        {
            _modules.Add(module);
            if (_dependencyInjector != null)
            {
                module.SetDependencyInjector(_dependencyInjector);
            }
            return this;
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

            if (_dependencyInjector != null)
            {
                _dependencyInjector.InjectDependencies(system);
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
            //Inject dependencies before initializing systems
            InjectDependencies();
            foreach (var system in _initSystems)
            {
                system.Init();
            }
            // Initialize submodules after this module's systems
            foreach (var module in _modules)
            {
                module.Init();
            }
        }

        public void InjectDependencies()
        {
            foreach (var system in _initSystems)
            {
                _dependencyInjector.InjectDependencies(system);
            }
            foreach (var system in _tickSystems)
            {
                _dependencyInjector.InjectDependencies(system);
            }
            foreach (var module in _modules)
            {
                module.InjectDependencies();
            }
            foreach (var module in _modules)
            {
                module.InjectDependencies();
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
                // Tick submodules as part of this module's update
                foreach (var module in _modules)
                {
                    module.Tick();
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
            // Dispose submodules
            foreach (var module in _modules)
            {
                module.Dispose();
            }
        }
    }
}