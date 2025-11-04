using System;
using Project.Services.NetTransportService;
using VenusECS.Core.Pool;
using static Project.Services.VenusNetService.VenusNetService;

namespace Project.Services.VenusNetService
{
    //Contract, that will create worlds and replicate it's states between peers
    //For example, server world will override state of client world, when input prediction is failed, in that case server world will be used to rollback to the correct state
    //For other worlds, we will use delta replication to sync the state between clients through server world
    //On client side, we will use the server world snapshot to rollback to the correct state
    //So clients play in their own world, but can see the correct state of the server world, than contains all client inputs and resolved predictions
    public interface IVenusNetWorldsSynchronator
    {
        event Action OnServerSnapshotNeeded;
        event Action<long> OnDeltaReady;
        IVenusPools GetWorld(NetTransportPeer peer);
        IVenusPools GetDefaultWorld();
        void AddPeer(NetTransportPeer peer);
        void RemovePeer(NetTransportPeer peer);
        void ApplyDelta(NetTransportPeer peer, WorldDeltaPayload deltaPayload);
        void ApplySnapshot(NetTransportPeer peer, byte[] data);
        void Reset();
        long GetCurrentFrame();
    }
}