using MeterRead.Services.DTO.Requests;
using MeterRead.Services.DTO.Responses;

namespace MeterRead.Services.Interfaces;

public interface IMeterReadingService
{
    ResponseHeader SubmitReading(MeterReadingRequest request);
}
