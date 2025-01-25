using System.Net;
using System.Net.Sockets;

namespace Server;

internal class Server
{
    private Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

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
        }
    }
}
