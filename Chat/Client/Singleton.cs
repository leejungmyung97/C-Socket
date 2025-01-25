using System.Net;
using System.Net.Sockets;

namespace Client;

/// <summary>
/// 언제 어디서나 접근할 수 있는 객체.
/// 이 객체는 반드시 1개만 존재해야 한다.
/// </summary>
internal class Singleton
{
    public string Id { get; set; } = null!;
    public string Nickname { get; set; } = null!;
    public Socket Socket { get; } = new Socket(AddressFamily.InterNetwork,SocketType.Stream, ProtocolType.Tcp);

    private static Singleton? instance;
    public static Singleton Instance
    {
        get
        {
            if (instance == null)
                instance = new Singleton();
            return instance;
        }
    }

    private Singleton()
    {

    }
    public async Task LoginAsync()
    {
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.123.100"), 20000);
        await Socket.ConnectAsync(endPoint);
        ThreadPool.QueueUserWorkItem(ReceiveAsync);
    }

    private async void ReceiveAsync(object? sender)
    {
        Socket socket = (Socket)sender;
    }
}
