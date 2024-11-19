namespace MeterRead.Services.DTO.Requests;

public sealed class MeterConnectionRequest : object
{
    public string Mpan { get; set; }

    public decimal MeterReading { get; set; }
}
