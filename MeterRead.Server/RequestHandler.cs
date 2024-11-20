using MeterRead.Services;
using MeterRead.Services.DTO.Requests;
using MeterRead.Services.DTO.Responses;
using MeterRead.Services.Interfaces;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace MeterRead.Server;

public sealed class RequestHandler(
    IMeterConnectionService meterConnectionService,
    IMeterReadingService meterReadingService,
    IBillingService billingService) : IRequestHandler
{
    private readonly IMeterConnectionService _meterConnectionService = meterConnectionService;
    private readonly IMeterReadingService _meterReadingService = meterReadingService;
    private readonly IBillingService _billingService = billingService;

    public ServerResponse HandleRequest(NetworkStream requestStream)
    {
        var json = GetRequestJson(requestStream);

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

    private static string GetRequestJson(NetworkStream requestStream)
    {
        var input = new byte[1024];
        var readBytes = requestStream.Read(input, 0, input.Length);

        var trimmedData = new byte[readBytes];
        for (var i = 0; i < readBytes; i++)
        {
            trimmedData[i] = input[i];
        }

        return Encoding.UTF8.GetString(trimmedData, 0, trimmedData.Length);
    }

    private ServerResponse ProcessRequest(RequestType requestType, object requestData)
    {
        ResponseHeader result = requestType switch
        {
            RequestType.MeterConnected => _meterConnectionService.ConnectMeter(((JsonElement)requestData).Deserialize<MeterConnectionRequest>() ?? new MeterConnectionRequest()),
            RequestType.MeterRead => _meterReadingService.SubmitReading(((JsonElement)requestData).Deserialize<MeterReadingRequest>() ?? new MeterReadingRequest()),
            RequestType.GetBill => _billingService.CalculateBill(((JsonElement)requestData).Deserialize<GetBillRequest>() ?? new GetBillRequest()),
            _ => new ResponseHeader { Success = false, Data = new ErrorResponse("An unknown error occurred processing the request") }
        };

        if (result.Data is null)
            return new(new ErrorResponse("An unknown error occurred processing the request"));

        return new(result, result.TerminateConnection);
    }
}
