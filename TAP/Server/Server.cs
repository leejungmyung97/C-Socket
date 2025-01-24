using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class Server
{
    static async Task Main(string[] args)
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.200.120"), 20000);
        serverSocket.Bind(endPoint);
        serverSocket.Listen(1000);

        // TAP (작업 기반 비동기 패턴)

        while (true)
        {
            Socket clientSocket = await serverSocket.AcceptAsync();
            Console.WriteLine(clientSocket.RemoteEndPoint);
            ThreadPool.QueueUserWorkItem(ReadAsync, clientSocket);
        }
    }

    private static async void ReadAsync(object? sender)
    {
        Socket clientSocket = (Socket)sender;
        while (true)
        {
            byte[] buffer = new byte[256];
            int n1 = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
            if (n1 < 1)
            {
                Console.WriteLine("cliend disconnect");
                clientSocket.Dispose();
                return;
            }
            Console.WriteLine(Encoding.UTF8.GetString(buffer));
        }
    }
}