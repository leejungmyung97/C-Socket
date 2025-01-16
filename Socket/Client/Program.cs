using System.Net;
using System.Net.Sockets;

namespace Client;

internal class Program
{
    static void Main(string[] args)
    {
        Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.123.100"), 20000);

        // 서버소켓에 ip, port 할당
        clientSocket.Connect(endPoint);

        // 클라이언트들의 연결 요청을 대기하는 상태로 만듦
        // 백로그큐 = 클라이언트들의 연결 요청 대기실
        
    }
}
