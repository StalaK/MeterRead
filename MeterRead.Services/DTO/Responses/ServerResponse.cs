using System.Text.Json;

namespace MeterRead.Services.DTO.Responses;

public sealed class ServerResponse
{
    public string JsonData { get; set; }

    public bool TerminateConnection { get; set; }

    public ServerResponse(object data)
    {
        JsonData = JsonSerializer.Serialize(data);
        TerminateConnection = false;
    }

    public ServerResponse(object data, bool terminateConnection)
    {
        JsonData = JsonSerializer.Serialize(data);
        TerminateConnection = terminateConnection;
    }
}
