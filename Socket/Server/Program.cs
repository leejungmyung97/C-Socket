using System.Net;
using System.Net.Sockets;

namespace Server;


internal class Program
{
    static void Main(string[] args)
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.123.100"), 20000);
        
        // 서버소켓에 ip, port 할당
        serverSocket.Bind(endPoint);

        // 클라이언트들의 연결 요청을 대기하는 상태로 만듦
        // 백로그큐 = 클라이언트들의 연결 요청 대기실
        serverSocket.Listen(20);

        // 연결 요청을 수락
        // 클라이언트와 데이터 통신을 위해 소켓 생성
        Socket ClienSocket = serverSocket.Accept();

        Console.WriteLine("연결됨" + ClienSocket.RemoteEndPoint);
    }
}