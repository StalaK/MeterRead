using MeterRead.Services;
using MeterRead.Services.DTO.Requests;
using MeterRead.Services.DTO.Responses;
using MeterRead.Services.Interfaces;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MeterRead.Server;

public sealed class RequestHandler(IMeterConnectionService meterConnectionService) : IRequestHandler
{
    private readonly IMeterConnectionService _meterConnectionService = meterConnectionService;

    public ServerResponse HandleRequest(NetworkStream requestStream)
    {
        var input = new byte[1024];
        var readBytes = requestStream.Read(input, 0, input.Length);

        var trimmedData = new byte[readBytes];
        for (var i = 0; i < readBytes; i++)
        {
            trimmedData[i] = input[i];
        }

        var json = Encoding.UTF8.GetString(trimmedData, 0, trimmedData.Length);

        if (string.IsNullOrWhiteSpace(json))
        {
            Logger.LogError("Unable to parse the request as JSON");
            return new ServerResponse(new ErrorResponse("Unable to parse the request as JSON"));
        }

        var request = JsonSerializer.Deserialize<RequestHeader<object>>(json);

        if (request is not RequestHeader<object> requestHeader)
        {
            Logger.LogError("The request header is null");
            return new ServerResponse(new ErrorResponse("The request header is null"));
        }

        if (requestHeader.Data is null)
        {
            Logger.LogError("The request data is empty");
            return new ServerResponse(new ErrorResponse("The request data is empty"));
        }

        return ProcessRequest(requestHeader.RequestType, requestHeader.Data);
    }

    private ServerResponse ProcessRequest(RequestType requestType, object requestData)
    {
        ResponseHeader result = requestType switch
        {
            RequestType.MeterConnected => _meterConnectionService.ConnectMeter(((JsonElement)requestData).Deserialize<MeterConnectionRequest>() ?? new MeterConnectionRequest()),
            _ => new ResponseHeader { Success = false, Data = new ErrorResponse("An unknown error occurred processing the request") }
        };

        if (result.Data is null)
            return new(new ErrorResponse("An unknown error occurred processing the request"));

        return new(result.Data, result.TerminateConnection);
    }
}
