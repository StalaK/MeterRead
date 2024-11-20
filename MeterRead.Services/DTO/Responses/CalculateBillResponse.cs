namespace MeterRead.Services.DTO.Responses;
public sealed class CalculateBillResponse(decimal currentTotal)
{
    public decimal CurrentTotal { get; set; } = currentTotal;
}
