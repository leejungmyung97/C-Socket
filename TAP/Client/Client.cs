using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Client;

internal class Client
{
    static async Task Main(string[] args)
    {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.200.120"), 20000);

        await socket.ConnectAsync(endPoint);

        while (true)
        {
            string str = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            await socket.SendAsync(buffer, SocketFlags.None);
        }
    }
}