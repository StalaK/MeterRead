namespace MeterRead.Services.DTO.Responses;
public sealed class ResponseHeader<T> : IResponse
{
    public bool Success { get; set; }

    public T? Data { get; set; }
}
