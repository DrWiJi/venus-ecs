using System;
using Unity.Collections.LowLevel.Unsafe;

namespace Project.Services.VenusNetService
{

    public partial class VenusNetService
    {
        internal struct WorldDeltaPayload
        {
            public long Frame;
            public byte[] Data;

            internal byte[] ToByteArray()
            {
                var frameBytes = new byte[8];
                unsafe
                {
                    fixed (byte* ptr = frameBytes)
                    {
                        *(long*)ptr = Frame;
                    }
                }
                var totalBytes = new byte[frameBytes.Length + Data.Length];
                Buffer.BlockCopy(frameBytes, 0, totalBytes, 0, frameBytes.Length);
                Buffer.BlockCopy(Data, 0, totalBytes, frameBytes.Length, Data.Length);
                return totalBytes;
            }

            internal static WorldDeltaPayload FromByteArray(byte[] data)            
            {
                unsafe
                {
                    fixed (byte* ptr = data)
                    {
                        var frame = *(long*)ptr;
                        int dataArraySize = data.Length - sizeof(long);
                        var dataBytes = new byte[dataArraySize];
                        UnsafeUtility.MemCpy(UnsafeUtility.AddressOf(ref dataBytes[0]), ptr + sizeof(long), dataArraySize);
                        return new WorldDeltaPayload { Frame = frame, Data = dataBytes };
                    }
                }
            }
        }
    }
}