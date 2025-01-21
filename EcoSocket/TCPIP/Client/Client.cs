using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client
{
    internal class Client
    {
        static void Main(string[] args)
        {
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("192.168.123.100"), 20000);
                socket.Connect(endPoint);

                while (true)
                {
                    string str = Console.ReadLine();
                    if (str == "exit")
                    {
                        return;
                    }

                    byte[] strBuffer = Encoding.UTF8.GetBytes(str);
                    byte[] newBuffer = new byte[2 + strBuffer.Length];
                    byte[] dataSize = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((short)strBuffer.Length));
                    
                    Array.Copy(dataSize, 0, newBuffer, 0, dataSize.Length);
                    Array.Copy(strBuffer, 0, newBuffer, 2, strBuffer.Length);
                    
                    socket.Send(newBuffer);

                    /*
                    byte[] buffer2 = new byte[256];
                    int bytesRead = socket.Receive(buffer2);
                    if (bytesRead < 1)
                    {
                        Console.WriteLine("서버의 연결 종료");
                        return;
                    }

                    string str2 = Encoding.UTF8.GetString(buffer2);
                    Console.WriteLine("받음 : " + str2);
                    */
                }
            }
        }
    }
}
