using UnityEngine;
using VContainer;
using LiteNetLib;
using LiteNetLib.Utils;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Project.Services.NetTransportService
{   
    public class NetTransport : INetTransport, INetEventListener
    {
        private const string ClientToServerKey = "ClientConnectToServerKey";
        private const int DefaultServerPort = 27015;

        private readonly object _stateLock = new object();
        private NetManager _manager;
        private NetPeer _clientToServerConnectionPeer;
        private bool _isServer;
        private int _serverPort = DefaultServerPort;
        private CancellationTokenSource _pollLoopCts;
        private Dictionary<long, NetPeer> _peersById = new Dictionary<long, NetPeer>();

        [Inject]
        public NetTransport()
        {
        }

        public event Action<bool> OnConnectionSuccess;
        public event Action<NetTransportPeer> OnClientConnected;
        public event Action<NetTransportPeer> OnClientDisconnected;
        public event Action<NetTransportPeer, NetDataPayload> OnMessageReceived;
        public event Action<NetTransportPeer> OnConnectedToGameServer;

        public void ConnectToGameServer(string host, int port)
        {
            lock (_stateLock)
            {
                _isServer = false;
                EnsureManagerStarted(clientMode: true);
                _clientToServerConnectionPeer = _manager.Connect(host, port, ClientToServerKey);
                if (_clientToServerConnectionPeer != null)
                {
                    OnConnectedToGameServer?.Invoke(new NetTransportPeer(_clientToServerConnectionPeer.Id));
                }
                else
                {
                    OnConnectionSuccess?.Invoke(false);
                }
            }
            // success will be reported from OnPeerConnected / failure from OnNetworkError
        }

        public void DisconnectFromGameServer()
        {
            lock (_stateLock)
            {
                if (_clientToServerConnectionPeer != null)
                {
                    _clientToServerConnectionPeer.Disconnect();
                    _clientToServerConnectionPeer = null;
                }
                TryStopManagerIfIdle();
            }
        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            if (_isServer)
            {
                request.AcceptIfKey(ClientToServerKey);
            }
            else
            {
                request.Reject();
            }
        }

        public void OnNetworkError(IPEndPoint endPoint, SocketError socketError)
        {
            Debug.LogError($"[NetTransport] Network error {socketError} at {endPoint}");
            OnConnectionSuccess?.Invoke(false);
        }

        public void OnNetworkLatencyUpdate(NetPeer peer, int latency)
        {
            // Optional: expose latency elsewhere if needed
        }

        public void OnNetworkReceive(NetPeer peer, NetPacketReader reader, byte channelNumber, DeliveryMethod deliveryMethod)
        {
            try
            {
                // Expect first 4 bytes as message type, remainder as payload
                if (reader.AvailableBytes < sizeof(int))
                {
                    return;
                }
                int type = reader.GetInt();
                int remaining = reader.AvailableBytes;
                byte[] data = remaining > 0 ? new byte[remaining] : Array.Empty<byte>();
                if (remaining > 0)
                {
                    reader.GetBytes(data, remaining);
                }

                var payload = new NetDataPayload();
                payload.SetData(data, type);
                var sender = _isServer ? new NetTransportPeer(peer.Id) : new NetTransportPeer(0);
                OnMessageReceived?.Invoke(sender, payload);
            }
            finally
            {
                reader.Recycle();
            }
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetPacketReader reader, UnconnectedMessageType messageType)
        {
            // Not used in current design
            reader.Recycle();
        }

        public void OnPeerConnected(NetPeer peer)
        {
            if (!_isServer)
            {
                lock (_stateLock)
                {
                    _peersById[peer.Id] = peer;
                }
                OnConnectionSuccess?.Invoke(true);
            }
            else
            {
                lock (_stateLock)
                {
                    if (!_peersById.ContainsKey(peer.Id))
                    {
                        _peersById[peer.Id] = peer;
                    }
                }
                OnClientConnected?.Invoke(new NetTransportPeer(peer.Id));
            }
        }

        public void OnPeerDisconnected(NetPeer peer, DisconnectInfo disconnectInfo)
        {
            if (!_isServer)
            {
                lock (_stateLock)
                {
                    if (_peersById.ContainsKey(peer.Id))
                    {
                        _peersById.Remove(peer.Id);
                    }
                }
                OnConnectionSuccess?.Invoke(false);
                TryStopManagerIfIdle();
            }
            else
            {
                lock (_stateLock)
                {
                    if (_peersById.ContainsKey(peer.Id))
                    {
                        _peersById.Remove(peer.Id);
                    }
                }
                OnClientDisconnected?.Invoke(new NetTransportPeer(peer.Id));
            }
        }

        public void SendMessageToGameServer(NetDataPayload payload)
        {
            NetPeer peer;
            lock (_stateLock)
            {
                peer = _clientToServerConnectionPeer;
            }
            if (peer == null)
            {
                Debug.LogWarning("[NetTransport] SendMessage called with no connected peer");
                return;
            }

            var writer = new NetDataWriter();
            writer.Put(payload.Type);
            if (payload.Data != null && payload.Length > 0)
            {
                writer.Put(payload.Data);
            }
            peer.Send(writer, DeliveryMethod.ReliableOrdered);
        }

        public void SendMessageToPeer(NetTransportPeer peer, NetDataPayload payload)
        {
            if (_manager == null)
            {
                return;
            }
            var writer = new NetDataWriter();
            writer.Put(payload.Type);
            if (payload.Data != null && payload.Length > 0)
            {
                writer.Put(payload.Data);
            }
            lock (_stateLock)
            {
                if (_peersById.ContainsKey(peer.Id))
                {
                    _peersById[peer.Id].Send(writer, DeliveryMethod.ReliableOrdered);
                }
            }
        }

        public void BroadcastToClients(NetDataPayload payload)
        {
            if (!_isServer || _manager == null)
            {
                return;
            }
            var writer = new NetDataWriter();
            writer.Put(payload.Type);
            if (payload.Data != null && payload.Length > 0)
            {
                writer.Put(payload.Data);
            }
            _manager.SendToAll(writer, DeliveryMethod.ReliableOrdered);
        }

        /// <summary>
        /// Test connection to a host and port, determines host type (game server or something else)
        /// </summary>
        public void TestConnection(string host, int port)
        {
            // Simple implementation: attempt to connect; report success/failure via OnConnectionSuccess
            ConnectToGameServer(host, port);
        }

        public void WaitForClientConnection(int port)
        {			
            lock (_stateLock)
            {
                _serverPort = port;
                _isServer = true;
                EnsureManagerStarted(clientMode: false);
            }
        }

        private void EnsureManagerStarted(bool clientMode)
        {
            if (_manager == null)
            {
                _manager = new NetManager(this)
                {
                    // Configure if needed (e.g., UnconnectedMessagesEnabled, UpdateTime, IPv6)
                };
            }

            if (!_manager.IsRunning)
            {
                if (clientMode)
                {
                    _manager.Start(); // client binds to an ephemeral local port
                }
                else
                {
                    _manager.Start(_serverPort);
                }
            }

            StartPollLoop();
        }

        private void StartPollLoop()
        {
            if (_pollLoopCts != null && !_pollLoopCts.IsCancellationRequested)
            {
                return;
            }
            _pollLoopCts = new CancellationTokenSource();
            var token = _pollLoopCts.Token;
            Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        NetManager mgr;
                        lock (_stateLock)
                        {
                            mgr = _manager;
                        }
                        if (mgr != null && mgr.IsRunning)
                        {
                            mgr.PollEvents();
                        }
                        await Task.Delay(15, token).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException)
                {
                    // expected on cancel
                }
            }, token);
        }

        private void TryStopManagerIfIdle()
        {
            if (_isServer)
            {
                return; // keep running to accept clients
            }

            if (_clientToServerConnectionPeer == null && _manager != null && _manager.IsRunning)
            {
                _manager.Stop();
                _pollLoopCts?.Cancel();
                _pollLoopCts = null;
            }
        }

        public void DisconnectClient(NetTransportPeer peer)
        {
            if (!_isServer || _manager == null) 
            {
                return;
            }
            foreach (var p in _manager.ConnectedPeerList)
            {
                if ((long)p.Id == peer.Id)
                {
                    p.Disconnect();
                    break;
                }
            }
        }        
    }
}