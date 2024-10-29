namespace MeterRead.Services.DTO.Requests;
public sealed class RequestHeader<T> : IRequest
{
    public RequestType RequestType { get; set; }

    public T? Data { get; set; }
}
