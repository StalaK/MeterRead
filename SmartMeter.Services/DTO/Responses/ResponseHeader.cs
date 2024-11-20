namespace SmartMeter.Services.DTO.Responses;
public sealed class ResponseHeader : IResponse
{
    public bool Success { get; set; }
    public bool TerminateConnection { get; set; }
    public required object Data { get; set; }
}
