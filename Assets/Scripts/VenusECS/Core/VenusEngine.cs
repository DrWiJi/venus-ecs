using System.Collections.Generic;

namespace VenusECS.Core
{
    public sealed class VenusEngine
    {
        private HashSet<VenusModule> _initedModules = new();
        private HashSet<VenusModule> _disposedModules = new();
        private HashSet<VenusModule> _modules = new();

        public VenusEngine(IEnumerable<VenusModule> modules)
        {
            foreach (var module in modules)
            {
                AddModule(module);
            }
        }

        public VenusEngine AddModule(VenusModule module)
        {
            _modules.Add(module);
            if (_initedModules.Count > 0)
            {
                module.Init();
                _initedModules.Add(module);
            }
            return this;
        }

        public VenusEngine RemoveModule(VenusModule module)
        {
            _modules.Remove(module);
            if (_initedModules.Count > 0)
            {
                module.Dispose();
                _initedModules.Remove(module);
            }
            return this;
        }

        public void InitEngine()
        {
            _disposedModules.Clear();
            foreach (var module in _modules)
            {
                module.Init();
                if (!_initedModules.Contains(module))
                    _initedModules.Add(module);
            }
        }

        public void Tick()
        {
            foreach (var module in _modules)
            {
                module.Tick();
            } 
        }

        public void DisposeEngine()
        {
            _initedModules.Clear();
            foreach (var module in _modules)
            {
                module.Dispose();
                if (!_disposedModules.Contains(module))
                    _disposedModules.Add(module);
            }
        }
    }
}