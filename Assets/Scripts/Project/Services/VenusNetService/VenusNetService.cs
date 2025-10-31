using UnityEngine;
using Project.Services.NetTransportService;
using VContainer;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project.Services.VenusNetService
{

    public partial class VenusNetService
    {
        private readonly INetTransport _netTransportService;
        private CancellationTokenSource _heartbeatCts;
        private int _heartbeatIntervalMs = 1000;
        private DateTime _lastHeartbeatSentUtc;
        private DateTime _lastHeartbeatAckUtc;
        private int _lastHeartbeatRttMs;

        private List<NetTransportPeer> _peers = new(10);
        private IVenusNetWorldsSynchronator _venusNetWorldsSynchronator;
        private Dictionary<NetTransportPeer, CancellationTokenSource> _clientWelcomeCtsByPeerId = new(10);

        [Inject]
        public VenusNetService(INetTransport netTransportService, IVenusNetWorldsSynchronator venusNetWorldsSynchronator)
        {
            _netTransportService = netTransportService;
            _venusNetWorldsSynchronator = venusNetWorldsSynchronator;
            _venusNetWorldsSynchronator.OnRequestServerSnapshot += OnRequestServerSnapshot;
            _venusNetWorldsSynchronator.OnDeltaReady += OnDeltaReady;
            _netTransportService.OnMessageReceived += OnTransportMessage;
            _netTransportService.OnClientConnected += OnClientConnected;
            _netTransportService.OnClientDisconnected += OnClientDisconnected;
            _netTransportService.OnConnectedToGameServer += OnConnectedToGameServer;
        }

        private void OnRequestServerSnapshot()
        {
            Debug.Log("RequestServerSnapshot received");
            var reqServerSnapshotPayload = new NetDataPayload();
            _venusNetWorldsSynchronator.GetDefaultWorld().GetSnapshot();
            reqServerSnapshotPayload.SetData(Array.Empty<byte>(), (int)NetMessageTypes.RequestServerSnapshot);
            _netTransportService.SendMessageToGameServer(reqServerSnapshotPayload);
        }

        ~VenusNetService()
        {
            if (_netTransportService != null)
            {
                _venusNetWorldsSynchronator.OnRequestServerSnapshot -= OnRequestServerSnapshot;
                _venusNetWorldsSynchronator.OnDeltaReady -= OnDeltaReady;
                _netTransportService.OnMessageReceived -= OnTransportMessage;
                _netTransportService.OnClientConnected -= OnClientConnected;
                _netTransportService.OnClientDisconnected -= OnClientDisconnected;
                _netTransportService.OnConnectedToGameServer -= OnConnectedToGameServer;
                foreach (var cts in _clientWelcomeCtsByPeerId.Values)
                {
                    cts.Cancel();
                }
                _clientWelcomeCtsByPeerId.Clear();
            }
        }


        public void StartServer(int port = 27015)
        {
            _netTransportService.WaitForClientConnection(port);
        }

        public void StartClient(string host, int port)
        {
            _netTransportService.ConnectToGameServer(host, port);
            StartHeartbeatLoop();
        }

        public void StopClient()
        {
            _heartbeatCts?.Cancel();
            _heartbeatCts = null;
            _netTransportService.DisconnectFromGameServer();
        }

        private void OnConnectedToGameServer(NetTransportPeer peer)
        {
            Debug.Log($"Connected to game server: {peer.Id}");
            //Send server welcome message to the client
            var serverWelcomePayload = new NetDataPayload();
            serverWelcomePayload.SetData(Array.Empty<byte>(), (int)NetMessageTypes.ClientWelcome);
            _netTransportService.SendMessageToPeer(peer, serverWelcomePayload);
        }

        private void OnDeltaReady(long frame)
        {
            //Send server delta to all clients
            var serverDeltaPayload = new NetDataPayload();
            serverDeltaPayload.SetData(Array.Empty<byte>(), (int)NetMessageTypes.WorldDelta);
            _netTransportService.SendMessageToGameServer(serverDeltaPayload);
            var deltaPayload = new NetDataPayload();
            var deltaPortionsData = _venusNetWorldsSynchronator.GetDefaultWorld().FlushDelta().deltaPortionsData;
            var worldDeltaPayload = new WorldDeltaPayload { Frame = frame, Data = deltaPortionsData };
            deltaPayload.SetData(worldDeltaPayload.ToByteArray(), (int)NetMessageTypes.WorldDelta);
            
            foreach (var peer in _peers)
            {
                _netTransportService.SendMessageToPeer(peer, deltaPayload);
            }
        }

        private void OnTransportMessage(NetTransportPeer sender, NetDataPayload payload)
        {
            switch (payload.Type)
            {
                case (int)NetMessageTypes.Heartbeat:
                    {
                        // Server side: echo back ack
                        if (sender.Id != 0)
                        {
                            var ack = new NetDataPayload();
                            ack.SetData(Array.Empty<byte>(), (int)NetMessageTypes.HeartbeatAck);
                            _netTransportService.SendMessageToPeer(sender, ack);
                        }

                        break;
                    }

                case (int)NetMessageTypes.HeartbeatAck:
                    {
                        var now = DateTime.UtcNow;
                        _lastHeartbeatAckUtc = now;
                        if (_lastHeartbeatSentUtc != default)
                        {
                            _lastHeartbeatRttMs = (int)(_lastHeartbeatAckUtc - _lastHeartbeatSentUtc).TotalMilliseconds;
                        }
                        Debug.Log($"HeartbeatAck received, rtt={_lastHeartbeatRttMs} ms");
                        break;
                    }

                case (int)NetMessageTypes.ServerWelcome:
                    Debug.Log("ServerWelcome received");
                    //Validate that the peer id is in the list
                    if (!_peers.Contains(sender))
                    {
                        Debug.LogError($"Peer id {sender.Id} not found in the list");
                        return;
                    }
                    //This is game server, so we need to add it to the list of peers
                    _venusNetWorldsSynchronator.AddPeer(sender);
                    //Also send server snapshot to the client
                    var serverSnapshotPayload = new NetDataPayload();
                    serverSnapshotPayload.SetData(_venusNetWorldsSynchronator.GetWorld(sender).GetSnapshot(), (int)NetMessageTypes.WorldSnapshot);
                    _netTransportService.SendMessageToPeer(sender, serverSnapshotPayload);
                    break;
                case (int)NetMessageTypes.ClientWelcome:
                    {
                        Debug.Log("ClientWelcome received");
                        //This is game client, so we need to add it to the list of peers
                        _venusNetWorldsSynchronator.AddPeer(sender);
                        //Send client welcome message to the server
                        var clientWelcomePayload = new NetDataPayload();
                        clientWelcomePayload.SetData(Array.Empty<byte>(), (int)NetMessageTypes.ServerWelcome);
                        _netTransportService.SendMessageToGameServer(clientWelcomePayload);
                        break;
                    }

                case (int)NetMessageTypes.WorldSnapshot:
                    Debug.Log("WorldSnapshot received");
                    _venusNetWorldsSynchronator.ApplySnapshot(sender, payload.Data);
                    break;
                case (int)NetMessageTypes.WorldDelta:
                    Debug.Log("WorldDelta received");
                    _venusNetWorldsSynchronator.ApplyDelta(sender, payload.Data);
                    break;
                case (int)NetMessageTypes.TimeSync:
                    Debug.Log("TimeSync received");
                    break;
                case (int)NetMessageTypes.RequestServerSnapshot:
                    Debug.Log("RequestServerSnapshot received");
                    var reqServerSnapshotPayload = new NetDataPayload();
                    _venusNetWorldsSynchronator.GetDefaultWorld().GetSnapshot();
                    reqServerSnapshotPayload.SetData(Array.Empty<byte>(), (int)NetMessageTypes.WorldSnapshot);
                    _netTransportService.SendMessageToPeer(sender, reqServerSnapshotPayload);
                    break;
                default:
                    break;

            }
        }

        private void OnClientConnected(NetTransportPeer peer)
        {
            Debug.Log($"Client connected: {peer.Id}");
            //Wait for client welcome message, if it's not received, disconnect the client
            _clientWelcomeCtsByPeerId[peer] = new CancellationTokenSource();
            _peers.Add(peer);
            Task.Run(async () =>
            {
                while (!_clientWelcomeCtsByPeerId[peer].IsCancellationRequested)
                {
                    await Task.Delay(1000, _clientWelcomeCtsByPeerId[peer].Token);
                }
                _netTransportService.DisconnectClient(peer);
            }, _clientWelcomeCtsByPeerId[peer].Token);
        }

        private void OnClientDisconnected(NetTransportPeer peer)
        {
            Debug.Log($"Client disconnected: {peer.Id}");
            //Validate that the peer id is in the list
            if (!_peers.Contains(peer))
            {
                Debug.LogError($"Peer id {peer.Id} not found in the list");
                return;
            }
            _peers.Remove(peer);
            _clientWelcomeCtsByPeerId.Remove(peer);
            Debug.Log($"Peer id {peer.Id} removed from the list");
        }

        private void StartHeartbeatLoop()
        {
            if (_heartbeatCts != null && !_heartbeatCts.IsCancellationRequested)
            {
                return;
            }

            _heartbeatCts = new CancellationTokenSource();
            var token = _heartbeatCts.Token;
            Task.Run(async () =>
            {
                try
                {
                    var payload = new NetDataPayload();
                    payload.SetData(Array.Empty<byte>(), (int)NetMessageTypes.Heartbeat);
                    while (!token.IsCancellationRequested)
                    {
                        _lastHeartbeatSentUtc = DateTime.UtcNow;
                        _netTransportService.SendMessageToGameServer(payload);
                        await Task.Delay(_heartbeatIntervalMs, token).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.Log("Heartbeat loop cancelled");
                }
            }, token);
        }
    }
}