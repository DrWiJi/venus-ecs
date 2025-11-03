using System;
using MessagePack;
using Unity.Collections.LowLevel.Unsafe;
using VenusECS.Core;

namespace Project.Services.VenusNetService
{

    public partial class VenusNetService
    {
        public struct WorldDeltaPayload
        {
            public long Frame;
            public (PoolsDeltaPortion[] deltaPortions, byte[] deltaPortionsData) Data;

            internal byte[] ToByteArray()
            {
                // Layout:
                // [0..7]   Frame (long)
                // [8..11]  DeltaPortionsCount (int)
                // [12..15] DeltaPortionsBytesLength (int)
                // [16..19] DeltaPortionsDataLength (int)
                // [..]     DeltaPortionsBytes (MessagePack of PoolsDeltaPortion[])
                // [..]     DeltaPortionsData (raw byte[])

                var portions = Data.deltaPortions ?? Array.Empty<PoolsDeltaPortion>();
                var portionsBytes = portions.Length > 0
                    ? MessagePackSerializer.Serialize(portions)
                    : Array.Empty<byte>();
                var rawData = Data.deltaPortionsData ?? Array.Empty<byte>();

                int headerSize = sizeof(long) + sizeof(int) + sizeof(int) + sizeof(int);
                int totalSize = headerSize + portionsBytes.Length + rawData.Length;
                var bytes = new byte[totalSize];

                int offset = 0;
                Buffer.BlockCopy(BitConverter.GetBytes(Frame), 0, bytes, offset, sizeof(long));
                offset += sizeof(long);
                Buffer.BlockCopy(BitConverter.GetBytes(portions.Length), 0, bytes, offset, sizeof(int));
                offset += sizeof(int);
                Buffer.BlockCopy(BitConverter.GetBytes(portionsBytes.Length), 0, bytes, offset, sizeof(int));
                offset += sizeof(int);
                Buffer.BlockCopy(BitConverter.GetBytes(rawData.Length), 0, bytes, offset, sizeof(int));
                offset += sizeof(int);

                if (portionsBytes.Length > 0)
                {
                    Buffer.BlockCopy(portionsBytes, 0, bytes, offset, portionsBytes.Length);
                    offset += portionsBytes.Length;
                }

                if (rawData.Length > 0)
                {
                    Buffer.BlockCopy(rawData, 0, bytes, offset, rawData.Length);
                }

                return bytes;
            }

            internal static WorldDeltaPayload FromByteArray(byte[] data)            
            {
                // Deserialize using the layout described above
                if (data == null || data.Length < sizeof(long))
                {
                    return new WorldDeltaPayload
                    {
                        Frame = 0,
                        Data = (Array.Empty<PoolsDeltaPortion>(), Array.Empty<byte>())
                    };
                }

                int offset = 0;
                long frame = BitConverter.ToInt64(data, offset);
                offset += sizeof(long);

                if (data.Length < sizeof(long) + sizeof(int) + sizeof(int) + sizeof(int))
                {
                    return new WorldDeltaPayload
                    {
                        Frame = frame,
                        Data = (Array.Empty<PoolsDeltaPortion>(), Array.Empty<byte>())
                    };
                }

                int portionsCount = BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                int portionsBytesLength = BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                int rawDataLength = BitConverter.ToInt32(data, offset);
                offset += sizeof(int);

                PoolsDeltaPortion[] portions = Array.Empty<PoolsDeltaPortion>();
                if (portionsBytesLength > 0)
                {
                    portions = MessagePackSerializer.Deserialize<PoolsDeltaPortion[]>(new ReadOnlyMemory<byte>(data, offset, portionsBytesLength));
                    offset += portionsBytesLength;
                }
                else if (portionsCount > 0)
                {
                    portions = new PoolsDeltaPortion[0];
                }

                byte[] rawData = Array.Empty<byte>();
                if (rawDataLength > 0)
                {
                    rawData = new byte[rawDataLength];
                    Buffer.BlockCopy(data, offset, rawData, 0, rawDataLength);
                }

                return new WorldDeltaPayload
                {
                    Frame = frame,
                    Data = (portions, rawData)
                };
            }
        }
    }
}