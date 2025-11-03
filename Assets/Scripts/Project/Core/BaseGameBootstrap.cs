using UnityEngine;
using Project.Services.VenusNetService;
using System;
using VenusECS.Core.Pool;
using Project.Services.NetTransportService;
using System.Collections.Generic;
using VenusECS.Core;
using static Project.Services.VenusNetService.VenusNetService;

namespace Project.Core
{
    public class BaseGameBootstrap : IVenusNetWorldsSynchronator
    {
        public event Action OnRequestServerSnapshot;
        public event Action<long> OnDeltaReady;
        protected Dictionary<NetTransportPeer, int> _peersWorldsIndices = new();

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
        }

        public void ApplyDelta(NetTransportPeer peer, WorldDeltaPayload deltaPayload)
        {
            var worldIndex = _peersWorldsIndices[peer];
            var world = Venus.SecondaryPools[worldIndex];
            //deserialize delta
            //var 
            //world.ApplyDelta(delta);
        }

        public void ApplySnapshot(NetTransportPeer peer, byte[] data)
        {
            throw new NotImplementedException();
        }

        public long GetCurrentFrame()
        {
            throw new NotImplementedException();
        }

        public IVenusPools GetDefaultWorld()
        {
            throw new NotImplementedException();
        }

        public IVenusPools GetWorld(NetTransportPeer peer)
        {
            throw new NotImplementedException();
        }

        public void RemovePeer(NetTransportPeer peer)
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}