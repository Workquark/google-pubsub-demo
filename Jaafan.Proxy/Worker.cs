using Jaafan.Proxy;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ServiceProxy
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly UdpClient receivingUdpClient = new UdpClient(9012);
        private readonly IPubSub pubsub;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
            pubsub = new PubSub();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //Creates an IPEndPoint to record the IP Address and port number of the sender.
                // The IPEndPoint will allow you to read datagrams sent from any source.
                IPEndPoint RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
                try
                {

                    // Blocks until a message returns on this socket from a remote host.
                    var udpReceiveResult = await receivingUdpClient.ReceiveAsync();
                    Byte[] receiveBytes = udpReceiveResult.Buffer;

                    string returnData = Encoding.ASCII.GetString(receiveBytes);

                    Console.WriteLine($"This is the message you received {returnData.ToString()}");
                    Console.WriteLine($"This message was sent from { RemoteIpEndPoint.Address.ToString() }  on their port number { RemoteIpEndPoint.Port.ToString() }");

                    await pubsub.PublishMessageWithCustomAttributesAsync("Amastasis", "my-iot-topic", returnData);

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
