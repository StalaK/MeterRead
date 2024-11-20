namespace MeterRead.Services.DTO.Requests;
public sealed class MeterReadingRequest
{
    public string Mpan { get; set; }

    public decimal MeterReading { get; set; }
}
