namespace SmartMeter.Services.DTO.Responses;

public sealed class MeterReadingResponse(string mpan, decimal meterReading)
{
    public string Mpan { get; } = mpan;

    public decimal MeterReading { get; } = meterReading;
}
