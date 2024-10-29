using System.Net;
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Client");

var ipEndpoint = new IPEndPoint(IPAddress.Loopback, 9113);

using TcpClient client = new();
await client.ConnectAsync(ipEndpoint);
await using NetworkStream stream = client.GetStream();

while (true)
{
    var buffer = new byte[1024];
    int receieved = await stream.ReadAsync(buffer);

    var message = Encoding.UTF8.GetString(buffer, 0, receieved);

    Console.WriteLine($"Received: {message}");

    Random rnd = new Random();
    while (true)
    {
        Thread.Sleep(1000);

        var num = rnd.Next(1000);

        Console.WriteLine($"Sending number: {num}");
        await stream.WriteAsync(Encoding.UTF8.GetBytes(num.ToString()));
    }
}