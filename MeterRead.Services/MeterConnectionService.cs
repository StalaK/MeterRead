using MeterRead.Services.DTO.Requests;
using MeterRead.Services.DTO.Responses;
using MeterRead.Services.Interfaces;

namespace MeterRead.Services;
public sealed class MeterConnectionService(IDatabase database) : IMeterConnectionService
{
    private readonly IDatabase _database = database;

    public ResponseHeader ConnectMeter(MeterConnectionRequest request)
    {
        if (string.IsNullOrEmpty(request.Mpan))
        {
            Logger.LogError("No MPAN was sent in the meter connection request");
            return new() { Success = false, Data = new ErrorResponse("No MPAN was sent in the meter connection request") };
        }

        var mpanValid = _database.ValidClient(request.Mpan);

        if (!mpanValid)
        {
            Logger.LogError($"The entered MPAN ({request.Mpan}) is not registered");
            return new() { Success = false, Data = new ErrorResponse($"The entered MPAN ({request.Mpan}) is not registered") };
        }

        _database.RecordReading(request.Mpan, request.MeterReading, 0m);

        Console.WriteLine($"Meter MPAN {request.Mpan} connected successfully with an opening reading of {request.MeterReading}");

        return new() { Success = true, TerminateConnection = false, Data = new MeterConnectionResponse(request.Mpan, request.MeterReading) };
    }
}
