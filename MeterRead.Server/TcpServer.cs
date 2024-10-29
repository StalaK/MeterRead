using MeterRead.Services.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MeterRead.Server;

public sealed class TcpServer : IServer
{
    private readonly IDatabase? _database;

    private TcpListener Server = new TcpListener(IPAddress.Any, 9113);
    private int ActiveConnections = 0;

    public void Start()
    {
        Console.WriteLine("Meter Reading Server");

        Server.Start();

        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine("Meter reading server started successfully!");
        Console.ForegroundColor = ConsoleColor.White;

        while (true)
            Server.BeginAcceptTcpClient(HandleConnection, Server);
    }

    private void HandleConnection(IAsyncResult result)
    {
        Server.BeginAcceptTcpClient(HandleConnection, Server);

        var client = Server.EndAcceptTcpClient(result);
        ActiveConnections++;

        Console.WriteLine($"New Client Connected! ({ActiveConnections})");
        Console.WriteLine($"Something from the DB: {_database.ValidClient("1234")}");
        Console.WriteLine($"Something else from the DB: {_database.ValidClient("1111110000000")}");

        while (true)
        {
            var stream = client.GetStream();

            var response = Encoding.UTF8.GetBytes("You are now connected to the server!");

            stream.Write(response, 0, response.Length);

            while (client.Connected)
            {
                var input = new byte[1024];
                stream.Read(input, 0, input.Length);

                Console.WriteLine($"{Encoding.UTF8.GetString(input)} - ({ActiveConnections} connections)");
            }
        }
    }
}
