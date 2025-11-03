using System;

namespace Project.Services.NetTransportService
{
    public interface INetTransport
    {
        event Action<bool> OnConnectionSuccess; // client connect/disconnect result
        event Action<NetTransportPeer> OnClientConnected; // server-side
        event Action<NetTransportPeer> OnClientDisconnected; // server-side
        event Action<NetTransportPeer, NetDataPayload> OnMessageReceived; // both sides
        event Action<NetTransportPeer> OnConnectedToGameServer; // client-side
        void TestConnection(string host, int port);
        void ConnectToGameServer(string host, int port);
        void DisconnectFromGameServer();
        void WaitForClientConnection(int port);
        void SendMessageToGameServer(NetDataPayload payload);
        void SendMessageToPeer(NetTransportPeer peer, NetDataPayload payload);
        void BroadcastToClients(NetDataPayload payload);
        void DisconnectClient(NetTransportPeer peer);
    }

    public class NetDataPayload
    {
        public int Type { get; set; }
        public byte[] Data { get; private set; }
        public int Length { get; private set; }
        public int Offset { get; set; }
        public int Position { get; set; }

        public NetDataPayload()
        {
            Data = new byte[1024];
            Length = 0;
            Type = -1;
            Offset = 0;
            Position = 0;
        }

        public void Reset()
        {
            Length = 0;
            Type = -1;
            Offset = 0;
            Position = 0;
        }
        
        public void SetData(byte[] data, int type)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (type <= 0)
            {
                throw new ArgumentException($"Type {type} is not valid. {nameof(type)} must be greater than 0.");
            }

            Data = data;
            Length = data.Length;
            Type = type;
            Offset = 0;
            Position = 0;
        }
    }

    public readonly struct NetTransportPeer
    {
        public long Id { get; }

        public NetTransportPeer(long id)
        {
            Id = id;
        }
    }
}