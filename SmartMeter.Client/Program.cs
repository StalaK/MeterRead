using SmartMeter.Services.DTO.Requests;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

string mpan = GetMpan();
Console.Title = $"Smart Meter Client (MPAN: {mpan})";
Console.WriteLine("Client");

var ipEndpoint = new IPEndPoint(IPAddress.Loopback, 9113);

using TcpClient client = new();
await client.ConnectAsync(ipEndpoint);
await using NetworkStream stream = client.GetStream();

while (true)
{
    Random rnd = new Random();
    var num = rnd.Next(1000);

    var connectionData = new MeterConnectionRequest { Mpan = mpan, MeterReading = num };
    var conn = new RequestHeader<MeterConnectionRequest>(connectionData);

    var connStr = JsonSerializer.Serialize(conn);
    Console.WriteLine($"Connecting with opening reading of {num}");
    await stream.WriteAsync(Encoding.UTF8.GetBytes(connStr));

    while (true)
    {
        Thread.Sleep(1000);
        Console.WriteLine("TODO: Send reading");
    }
}

string GetMpan()
{
    const int MPAN_LENGTH = 13;
    var clientProcessCount = Process.GetProcesses().Count(x => x.ProcessName.Equals("SmartMeter.Client"));
    return clientProcessCount.ToString().PadRight(MPAN_LENGTH, '0');
}