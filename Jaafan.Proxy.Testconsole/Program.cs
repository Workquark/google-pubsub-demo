using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Application.ServiceProxy.Testconsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await JsonTest();

            async Task JsonTest()
            {
                List<byte[]> byteList = new List<byte[]>();
                string json = File.ReadAllText("hextest.json");
                Console.WriteLine($"JSON  STRING : {json}");
                byte[] arr = Encoding.UTF8.GetBytes(json);
                await SendHex(arr);
            }
        }

        public static async Task SendHex(byte[] bytes)
        {
            UdpClient client = new UdpClient(0);
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9012);
            //IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse("20.53.116.216"), 9012);
            int i = 0;
            while (true)
            {
                await client.SendAsync(bytes, bytes.Length, endPoint);
                Console.WriteLine($"{++i}: message send");
                //await Task.Delay(2000);
                break;
            }
        }
    }
}
