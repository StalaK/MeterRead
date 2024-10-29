namespace MeterRead.Services.DTO.Responses;

public sealed class MeterConnectionResponse(string mpan, decimal meterReading)
{
    public string Mpan { get; } = mpan;

    public decimal MeterReading { get; } = meterReading;
}
