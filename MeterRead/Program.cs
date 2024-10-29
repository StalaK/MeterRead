using MeterRead;
using MeterRead.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder();
builder.RegisterDependencies();
using var host = builder.Build();

var server = host.Services.GetService<IServer>() ?? throw new ArgumentNullException();
server.Start();

await host.RunAsync();
