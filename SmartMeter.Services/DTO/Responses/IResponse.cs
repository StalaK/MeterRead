namespace SmartMeter.Services.DTO.Responses;

public interface IResponse
{
    public bool TerminateConnection { get; set; }
    public object Data { get; set; }
}
