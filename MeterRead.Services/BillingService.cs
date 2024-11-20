using MeterRead.Services.DTO.Requests;
using MeterRead.Services.DTO.Responses;
using MeterRead.Services.Interfaces;

namespace MeterRead.Services;

public sealed class BillingService(IDatabase database) : IBillingService
{
    private readonly IDatabase _database = database;

    public ResponseHeader CalculateBill(GetBillRequest request)
    {
        var meterReadings = _database.GetMeterReadings(request.Mpan);

        if (meterReadings.Count() <= 1)
        {
            Logger.LogWarning($"No meter readings found for the MPAN {request.Mpan}");
            return new() { Success = false, TerminateConnection = false, Data = new CalculateBillResponse(0) };
        }

        var billTotal = 0m;
        var previousReading = -1m;

        foreach (var reading in meterReadings.OrderBy(x => x.ReadingTime))
        {
            if (previousReading == -1)
            {
                previousReading = reading.Reading;
                continue;
            }

            var usage = reading.Reading - previousReading;

            if (usage < 0)
            {
                Logger.LogError($"The MPAN {request.Mpan} submitted a reading (Reading ID: {reading.Id} which resulted in negative usage. Skipping this reading from billing calculations");
                continue;
            }

            billTotal += usage * reading.Rate;

            previousReading = reading.Reading;
        }

        return new() { Success = true, TerminateConnection = false, Data = new CalculateBillResponse(billTotal) };
    }
}
