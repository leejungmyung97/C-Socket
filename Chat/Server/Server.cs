using Core;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

namespace Server;

internal class Server
{
    private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    public ConcurrentDictionary<string, Room> Rooms { get; } = new ConcurrentDictionary<string, Room>();

    public Server(string ip, int port, int backlog)
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(backlog);
    }

    public async Task StartAsync()
    {
        while (true)
        {
            Socket clientSocket = await serverSocket.AcceptAsync();
            Console.WriteLine(clientSocket.RemoteEndPoint);
            ThreadPool.QueueUserWorkItem(RunAsync, clientSocket);
        }
    }

    private async void RunAsync(object? sender)
    {
        Socket clientSocket = (Socket)sender!;
        byte[] headerBuffer = new byte[2];

        string id = "";
        string nickname = "";
        string roomName = "";

        while (true)
        {
            #region 헤더버퍼 가져옮
            int n1 = await clientSocket.ReceiveAsync(headerBuffer, SocketFlags.None);
            if (n1 < 1)
            {
                Console.WriteLine("client disconnect");
                clientSocket.Dispose();
                return;
            }
            else if (n1 == 1)
            {
                await clientSocket.ReceiveAsync(new ArraySegment<byte>(headerBuffer, 1, 1), SocketFlags.None);
            }
            #endregion

            #region 데이터버퍼 가져옮
            short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headerBuffer));
            byte[] dataBuffer = new byte[dataSize];

            int totalRecv = 0;
            while (totalRecv < dataSize)
            {
                int n2 = await clientSocket.ReceiveAsync(new ArraySegment<byte>(dataBuffer, totalRecv, dataSize - totalRecv), SocketFlags.None);
                totalRecv += n2;
            }
            #endregion

            PacketType packetType = (PacketType)IPAddress.NetworkToHostOrder(BitConverter.ToInt16(dataBuffer));
            if (packetType == PacketType.LoginRequest)
            {
                LoginRequestPacket packet = new LoginRequestPacket(dataBuffer);
                Console.WriteLine($"id:{packet.Id} nickname:{packet.Nickname}");

                id = packet.Id;
                nickname = packet.Nickname;
                // 200 = 응답성공
                LoginResponsePacket packet2 = new LoginResponsePacket(200);
                await clientSocket.SendAsync(packet2.Serialize(), SocketFlags.None);
            }
            else if (packetType == PacketType.CreateRoomRequest)
            {
                CreateRoomRequestPacket packet = new CreateRoomRequestPacket(dataBuffer);
                Room room = new Room();

                if (Rooms.TryAdd(packet.RoomName, room))
                {
                    roomName = packet.RoomName;
                    room.Users.TryAdd(id, nickname);
                    Console.WriteLine("created room : " + roomName);
                    CreateRoomResponsePacket packet2 = new CreateRoomResponsePacket(200);
                    await clientSocket.SendAsync(packet2.Serialize(), SocketFlags.None);
                }
                else
                {
                    Console.WriteLine("created failed");
                    CreateRoomResponsePacket packet2 = new CreateRoomResponsePacket(500);
                    await clientSocket.SendAsync(packet2.Serialize(), SocketFlags.None);
                }
            }
            else if (packetType == PacketType.RoomListRequest)
            {
                RoomListResponsePacket packet = new RoomListResponsePacket(Rooms.Keys);
                await clientSocket.SendAsync(packet.Serialize(), SocketFlags.None);
            }
        }
    }
}
