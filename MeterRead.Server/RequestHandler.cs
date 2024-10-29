using MeterRead.Services;
using MeterRead.Services.DTO.Requests;
using MeterRead.Services.DTO.Responses;
using MeterRead.Services.Interfaces;

namespace MeterRead.Server;

internal sealed class RequestHandler(IMeterConnectionService meterConnectionService)
{
    private readonly IMeterConnectionService _meterConnectionService = meterConnectionService;

    public IResponse HandleRequest(IRequest request)
    {
        var requestHeader = request as RequestHeader<object>;

        if (requestHeader is null)
        {
            Logger.LogError("The request header is null");
            return new ResponseHeader<string> { Success = false };
        }

        return requestHeader.RequestType switch
        {
            RequestType.MeterConnected => _meterConnectionService.ConnectMeter(requestHeader.Data as MeterConnectionRequest ?? new MeterConnectionRequest()),
            _ => new ResponseHeader<string> { Success = false }
        };
    }
}
