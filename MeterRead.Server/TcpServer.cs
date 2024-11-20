using MeterRead.Services;
using MeterRead.Services.DTO.Responses;
using MeterRead.Services.Interfaces;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MeterRead.Server;

public sealed class TcpServer(IDatabase database, IRequestHandler requestHandler) : IServer
{
    private readonly IDatabase _database = database;
    private readonly IRequestHandler _requestHandler = requestHandler;

    private TcpListener Server = new TcpListener(IPAddress.Any, 9113);
    private int ActiveConnections = 0;

    public void Start()
    {
        Console.Title = "Meter Reading Server";

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

        var stream = client.GetStream();

        while (client.Connected)
        {
            try
            {
                var requestResult = _requestHandler.HandleRequest(stream);

                var responseJson = string.IsNullOrWhiteSpace(requestResult.JsonData)
                    ? JsonSerializer.Serialize(new ErrorResponse("An unknown error occurred with the request"))
                    : requestResult.JsonData;

                var responseData = Encoding.UTF8.GetBytes(responseJson);

                stream.Write(responseData, 0, responseData.Length);

                if (requestResult.TerminateConnection)
                    client.Close();
            }
            catch (Exception ex) when (ex.InnerException is SocketException)
            {
                ActiveConnections--;
                Logger.LogInformation("A client has disconnected");
            }
        }
    }
}
