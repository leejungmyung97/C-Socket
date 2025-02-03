using System.Net;
using System.Text;

namespace Core;

public class RoomListResponsePacket : IPacket
{
    public List<string> RoomNames { get; }

    public RoomListResponsePacket(ICollection<string> roomNames)
    {
        RoomNames = new List<string>(roomNames);
    }

    public RoomListResponsePacket(byte[] buffer)
    {
        RoomNames = new List<string>();
        for (int offset = 2; offset < buffer.Length;)
        {
            short roomNameSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
            offset += sizeof(short);

            RoomNames.Add(Encoding.UTF8.GetString(buffer, offset, roomNameSize));
            offset += roomNameSize;
        }
    }

    public byte[] Serialize()
    {
        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType.RoomListResponse));

        short dataSize = (short)(packetType.Length);

        List<byte[]> temp = new List<byte[]>();
        foreach (var item in RoomNames)
        {
            byte[] nameBuffer = Encoding.UTF8.GetBytes(item);
            byte[] nameSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)nameBuffer.Length));
            dataSize += (short)(nameBuffer.Length + nameSize.Length);
            temp.Add(nameSize);
            temp.Add(nameBuffer);
        }

        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        int offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;

        foreach (var item in temp)
        {
            Array.Copy(item, 0, buffer, offset, item.Length);
            offset += item.Length;
        }

        return buffer;
    }
}
