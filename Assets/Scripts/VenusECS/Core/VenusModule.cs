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