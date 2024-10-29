using MeterRead.Services.DTO.Requests;
using MeterRead.Services.DTO.Responses;
using MeterRead.Services.Interfaces;

namespace MeterRead.Services;
public sealed class MeterConnectionService(IDatabase database) : IMeterConnectionService
{
    private readonly IDatabase _database = database;

    public ResponseHeader<MeterConnectionResponse> ConnectMeter(MeterConnectionRequest request)
    {
        if (string.IsNullOrEmpty(request.Mpan))
        {
            Logger.LogError("No MPAN was sent in the meter connection request");
            return new() { Success = false };
        }

        var mpanValid = _database.ValidClient(request.Mpan);

        if (!mpanValid)
        {
            Logger.LogError($"The entered MPAN ({request.Mpan}) is not registered");
            return new() { Success = false };
        }

        _database.RecordReading(request.Mpan, request.MeterReading, 0m);

        return new() { Data = new(request.Mpan, request.MeterReading) };
    }
}
