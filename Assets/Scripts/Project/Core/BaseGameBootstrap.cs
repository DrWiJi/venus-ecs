using UnityEngine;
using Project.Services.VenusNetService;
using System;
using VenusECS.Core.Pool;
using Project.Services.NetTransportService;
using System.Collections.Generic;
using VenusECS.Core;
using static Project.Services.VenusNetService.VenusNetService;
using Project.Core.Gameplay.Systems;
using VContainer;
using Project.Services.Input;

namespace Project.Core
{
    public abstract class BaseGameBootstrap : MonoBehaviour, IVenusNetWorldsSynchronator
    {
        public event Action OnServerSnapshotNeeded;
        public event Action<long> OnDeltaReady;
        protected Dictionary<NetTransportPeer, int> _peersWorldsIndices = new();
        protected long _localSimulationFrame = 0;
        protected Dictionary<int, Queue<(NetTransportPeer peer, long frame, long receiveFrame, WorldDeltaPayload deltaPayload)>> _deltaQueues = new(256);
        protected Dictionary<int, float> _jitterStats = new(256);
        
        //Local systems, that will be used to handle user input and make predictions
        protected VenusModule _gameFixedLogicModule = new(new AlwaysTrueUpdateResolver());
        //Local systems, that will be used to handle view logic
        protected VenusModule _gameViewLogicModule = new(new AlwaysTrueUpdateResolver());
        //Local systems, that will be used to handle replication logic, that will be used to sync the state between clients through server world
        protected VenusModule _replicationModule = new(new AlwaysTrueUpdateResolver());

        private VenusEngine _venusFixedLogicEngine;
        private VenusEngine _venusViewLogicEngine;
        private VenusEngine _venusReplicationEngine;

        private void Awake()
        {
            _venusFixedLogicEngine = new VenusEngine(new VenusModule[] {
                _gameFixedLogicModule,
            });
            _venusViewLogicEngine = new VenusEngine(new VenusModule[] {
                _gameViewLogicModule,
            });
            _venusReplicationEngine = new VenusEngine(new VenusModule[] {
                _replicationModule,
            });
            OnAwake();
        }

        protected abstract void OnAwake();

        private void Start()
        {
            OnStart();
            _venusFixedLogicEngine.InitEngine();
            _venusViewLogicEngine.InitEngine();
            _venusReplicationEngine.InitEngine();
        }
        protected abstract void OnStart();

        private void Update()
        {
            foreach(var kvp in _peersWorldsIndices)
            {
                var index = kvp.Value;
                Venus.SwitchSecondaryPool(index);
                _venusReplicationEngine.Tick();
            }
            _venusViewLogicEngine.Tick();
            OnUpdate();
        }

        protected abstract void OnUpdate();

        private void FixedUpdate()
        {
            DequeueDelta();
            _venusFixedLogicEngine.Tick();
            OnFixedUpdate();
            OnDeltaReady?.Invoke(_localSimulationFrame);
            _localSimulationFrame++;
        }
        protected abstract void OnFixedUpdate();

        private void DequeueDelta()
        {
            //Dequeue next delta for peer world
            foreach (var kvp in _peersWorldsIndices)
            {
                var worldIndex = kvp.Value;
                var deltaQueue = _deltaQueues[worldIndex];
                //Must keep in mind jitter, so there must be a few frames delay between the delta and the local frame, that count must depend on the jitter
                var jitter = _jitterStats[worldIndex];
                // Decide how many deltas to leave in the queue based on jitter (ceiling), clamped to [1, 4)
                int leaveCount = Mathf.CeilToInt(jitter);
                leaveCount = Mathf.Clamp(leaveCount, 1, 5);

                int toApply = Mathf.Max(0, deltaQueue.Count - leaveCount);
                if (toApply > 0)
                {
                    var world = Venus.SecondaryPools[worldIndex];
                    for (int i = 0; i < toApply; i++)
                    {
                        var item = deltaQueue.Dequeue();
                        world.ApplyDelta(item.deltaPayload.Data.deltaPortions, item.deltaPayload.Data.deltaPortionsData);
                    }
                }
            }
        }

        public void AddPeer(NetTransportPeer peer)
        {
            //Create new world for the peer
            //before, check if the peer already has a world
            if (_peersWorldsIndices.ContainsKey(peer))
            {
                throw new Exception($"Peer {peer.Id} already has a world");
            }
            var worldIndex = Venus.CreateSecondaryPools();
            _peersWorldsIndices[peer] = worldIndex;
            _deltaQueues[worldIndex] = new Queue<(NetTransportPeer peer, long frame, long receiveFrame, WorldDeltaPayload deltaPayload)>(256);
            _jitterStats[worldIndex] = 0f;
        }

        public void ApplyDelta(NetTransportPeer peer, WorldDeltaPayload deltaPayload)
        {
            var worldIndex = _peersWorldsIndices[peer];
            _deltaQueues[worldIndex].Enqueue((peer, deltaPayload.Frame, _localSimulationFrame, deltaPayload));
            //Calculate jitter with the last delta
            var lastDelta = _deltaQueues[worldIndex].Peek();
            var jitter = _localSimulationFrame - lastDelta.receiveFrame;
            //New jitter value weight is 0.1
            _jitterStats[worldIndex] = 0.1f * jitter + 0.9f * _jitterStats[worldIndex];
        }

        public void ApplySnapshot(NetTransportPeer peer, byte[] data)
        {
            var worldIndex = _peersWorldsIndices[peer];
            var world = Venus.SecondaryPools[worldIndex];
            world.RestoreSnapshot(data);
        }

        public long GetCurrentFrame()
        {
            return _localSimulationFrame;
        }

        public IVenusPools GetDefaultWorld()
        {
            return Venus.Pools;
        }

        public IVenusPools GetWorld(NetTransportPeer peer)
        {
            var worldIndex = _peersWorldsIndices[peer];
            return Venus.SecondaryPools[worldIndex];
        }

        public void RemovePeer(NetTransportPeer peer)
        {
            var worldIndex = _peersWorldsIndices[peer];
            _deltaQueues.Remove(worldIndex);
            Venus.RemoveSecondaryPool(worldIndex);
            _peersWorldsIndices.Remove(peer);
        }

        public void Reset()
        {
            foreach(var worldIndex in _peersWorldsIndices.Values)
            {
                Venus.RemoveSecondaryPool(worldIndex);
            }
            _peersWorldsIndices.Clear();
            _deltaQueues.Clear();
            _jitterStats.Clear();
            _localSimulationFrame = 0;
        }
        
        private void OnDestroy()
        {
            _venusFixedLogicEngine.DisposeEngine();
            _venusViewLogicEngine.DisposeEngine();
            _venusReplicationEngine.DisposeEngine();
            OnDestroyed();
        }

        protected abstract void OnDestroyed();
    }

    public class ClientGameBootstrap : BaseGameBootstrap
    {
        protected override void OnAwake()
        {
            _gameFixedLogicModule
            .AddSystem(new InputReaderSystem())
            
            ;
        }

        [Inject]
        private void InjectDependencies(InputService inputService)
        {
            _gameFixedLogicModule.AddService(typeof(InputService), inputService);
        }

        protected override void OnFixedUpdate()
        {
        }

        protected override void OnStart()
        {
        }

        protected override void OnUpdate()
        {
            
        }

        protected override void OnDestroyed()
        {
        }
    }

    public class ServerGameBootstrap : BaseGameBootstrap
    {
        protected override void OnAwake()
        {
        }

        [Inject]
        private void InjectDependencies()
        {


        }

        protected override void OnFixedUpdate()
        {
        }

        protected override void OnStart()
        {
        }

        protected override void OnUpdate()
        {
        }

        protected override void OnDestroyed()
        {
        }
    }
}