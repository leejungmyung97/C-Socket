using System.Net;
using System.Text;

namespace ConverterTest;

internal class Program
{
    static void Main(string[] args)
    {
        // 직렬화: 메모리상에 있는 객체를 전송/저장 가능한 형태로 만드는 것
        Person person = new Person(); //JSON, Xml

        //역직렬화: 직렬화된 데이터를 다시 객체의 형태로 만드는것
        long num = 1234;
        byte[] buffer = BitConverter.GetBytes(num);
        Console.WriteLine(buffer.Length);

        long num2 = BitConverter.ToInt64(buffer, 0);
        Console.WriteLine(num2);

        string str = "hello world";
        byte[] buffer2 = Encoding.UTF8.GetBytes(str);
        Console.WriteLine(buffer2);

        string str2 = Encoding.UTF8.GetString(buffer2);

        Console.WriteLine(str2);

        //바이트 오더: 바이트를 메모리에 저장하는 순서
        byte[] buffer3 = BitConverter.GetBytes(12345678);
        Console.WriteLine(BitConverter.ToString(buffer3));

        //인텔/AMD: 리틀엔디언 - 하위 바이트 부터 저장
        //ARM계열: 빅엔디언 - 상위 바이트 부터 저장(이게 국룰)

        int num3 = IPAddress.HostToNetworkOrder(12345678);
        byte[] buffer4 = BitConverter.GetBytes(num3);
        Console.WriteLine(BitConverter.ToString(buffer4));

    }
}
