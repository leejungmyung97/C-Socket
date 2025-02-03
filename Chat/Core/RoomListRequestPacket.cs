using System.Net;

namespace Core;

public class RoomListRequestPacket : IPacket
{
    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType.RoomListRequest));

        short dataSize = (short)(packetType.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        int offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);

        return buffer;
    }
}
