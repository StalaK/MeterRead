using MeterRead.Services.DTO.Responses;
using System.Net.Sockets;

namespace MeterRead.Services.Interfaces;

public interface IRequestHandler
{
    ServerResponse HandleRequest(NetworkStream requestStream);
}
