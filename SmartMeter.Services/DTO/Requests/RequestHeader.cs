namespace SmartMeter.Services.DTO.Requests;

public sealed class RequestHeader<T>(T data) : IRequest where T : notnull
{
    public RequestType RequestType { get; set; } = data switch
    {
        MeterConnectionRequest => RequestType.MeterConnected,
        _ => throw new NotSupportedException()
    };

    public T Data { get; set; } = data;
}
