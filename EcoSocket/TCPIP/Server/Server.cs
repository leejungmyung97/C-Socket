using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server;

internal class Server
{
    static void Main(string[] args)
    {
        using (Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.123.100"), 20000);
            serverSocket.Bind(endPoint);
            serverSocket.Listen(20);

            using (Socket clientSocket = serverSocket.Accept())
            {
                Console.WriteLine(clientSocket.RemoteEndPoint);

                while (true)
                {
                    byte[] headerBuffer = new byte[2];
                    int n1 = clientSocket.Receive(headerBuffer);
                    if (n1 < 1)
                    {
                        Console.WriteLine("클라이언트의 연결종료");
                        return;
                    }
                    else if (n1 == 1)
                    {
                        clientSocket.Receive(headerBuffer,1,1,SocketFlags.None);
                    }

                    short dataSize = IPAddress.NetworkToHostOrder(BitConverter.ToInt16(headerBuffer));
                    byte[] dataBuffer = new byte[dataSize];

                    int totalRecv = 0;

                    while (totalRecv < dataSize)
                    {
                        int n2 = clientSocket.Receive(dataBuffer, totalRecv, dataSize - totalRecv, SocketFlags.None);
                        totalRecv += n2;
                    }

                    string str = Encoding.UTF8.GetString(dataBuffer);
                    Console.WriteLine(str);

                    //clientSocket.Send(buffer);
                }
            }
        }
    }
}
