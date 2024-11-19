namespace MeterRead.Services.DTO.Responses;

public sealed class ErrorResponse(string error)
{
    public string Error { get; } = error;

    public bool Success { get => false; }
}
