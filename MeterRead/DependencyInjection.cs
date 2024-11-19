using MeterRead.Database;
using MeterRead.Server;
using MeterRead.Services;
using MeterRead.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MeterRead;
internal static class DependencyInjection
{
    internal static void RegisterDependencies(this HostApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IDatabase, DatabaseStub>();
        builder.Services.AddSingleton<IServer, TcpServer>();

        builder.Services.AddTransient<IClientManager, ClientManagerService>();
        builder.Services.AddTransient<IInvoicing, InvoicingService>();
        builder.Services.AddTransient<IMeterReading, MeterReadingService>();
        builder.Services.AddTransient<IMeterConnectionService, MeterConnectionService>();
        builder.Services.AddTransient<IRequestHandler, RequestHandler>();
    }
}
