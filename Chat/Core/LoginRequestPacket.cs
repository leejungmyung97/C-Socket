using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core;

public class LoginRequestPacket : IPacket
{
    public string Id { get; private set; }
    public string Nickname { get; private set; }

    public LoginRequestPacket(string id, string nickname)
    {
        Id = id;
        Nickname = nickname;
    }

    public LoginRequestPacket(byte[] buffer)
    {
        int offset = 2;

        short idSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);

        Id = Encoding.UTF8.GetString(buffer, offset, idSize);
        offset += idSize;

        short nicknameSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(buffer, offset));
        offset += sizeof(short);

        Nickname = Encoding.UTF8.GetString(buffer, offset, nicknameSize);
    }

    public byte[] Serialize()
    {
        // 2바이트 헤더
        // 데이터
        // 2바이트 패킷타입
        // 2바이트 id size
        // id
        // 2바이트 nickname size
        // nickname

        byte[] packetType = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)PacketType.LoginRequest));
        byte[] id = Encoding.UTF8.GetBytes(Id);
        byte[] idSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)id.Length));
        byte[] nickname = Encoding.UTF8.GetBytes(Nickname);
        byte[] nicknameSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)nickname.Length));

        short dataSize = (short)(packetType.Length + id.Length + idSize.Length + nickname.Length + nicknameSize.Length);
        byte[] header = BitConverter.GetBytes(IPAddress.HostToNetworkOrder(dataSize));

        byte[] buffer = new byte[2 + dataSize];

        int offset = 0;

        Array.Copy(header, 0, buffer, offset, header.Length);
        offset += header.Length;

        Array.Copy(packetType, 0, buffer, offset, packetType.Length);
        offset += packetType.Length;

        Array.Copy(idSize, 0, buffer, offset, idSize.Length);
        offset += idSize.Length;

        Array.Copy(id, 0, buffer, offset, id.Length);
        offset += id.Length;

        Array.Copy(nicknameSize, 0, buffer, offset, nicknameSize.Length);
        offset += nicknameSize.Length;

        Array.Copy(nickname, 0, buffer, offset, nickname.Length);

        return buffer;
    }
}
