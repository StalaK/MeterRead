using SmartMeter.Services.DTO.Requests;
using SmartMeter.Services.DTO.Responses;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

string mpan = GetMpan();
Console.Title = $"Smart Meter Client (MPAN: {mpan})";
Console.WriteLine("Client");

var ipEndpoint = new IPEndPoint(IPAddress.Loopback, 9113);

using TcpClient client = new();
await client.ConnectAsync(ipEndpoint);
await using NetworkStream stream = client.GetStream();

while (true)
{
    Random rnd = new Random();
    var totalReading = rnd.Next(10000, 90000);

    var connectionRequest = GetConnectionRequestJson(mpan, totalReading);

    await stream.WriteAsync(Encoding.UTF8.GetBytes(connectionRequest));

    var connectionResponseJson = GetResponseJson(stream);

    var connectionResponse = JsonSerializer.Deserialize<ResponseHeader>(connectionResponseJson);

    if (connectionResponse?.Success == true)
    {
        Console.WriteLine($"Successfully connected with a meter reading of {totalReading}");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("The connection request was unsuccessful - Terminating connection!");
        Console.ForegroundColor = ConsoleColor.White;

        stream.Close();
        return;
    }

    if (connectionResponse?.TerminateConnection == true)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("The server has terminated the connection");
        Console.ForegroundColor = ConsoleColor.White;

        stream.Close();
        return;
    }

    var readingCount = 1;

    while (true)
    {
        Thread.Sleep(5000);

        var reading = rnd.Next(10);
        totalReading += reading;

        var meterReading = GetMeterReadingJson(mpan, totalReading);
        await stream.WriteAsync(Encoding.UTF8.GetBytes(meterReading));

        var responseJson = GetResponseJson(stream);

        var response = JsonSerializer.Deserialize<ResponseHeader>(responseJson);

        if (response?.Success == true)
        {
            Console.WriteLine($"Successfully submitted a meter reading of {totalReading}");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("The meter reading request was unsuccessful!");
            Console.ForegroundColor = ConsoleColor.White;
        }

        if (response?.TerminateConnection == true)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("The server has terminated the connection");
            Console.ForegroundColor = ConsoleColor.White;

            stream.Close();
            return;
        }

        if (readingCount % 5 == 0)
        {
            // Get bill
            var getBillJson = GetGetBillJson(mpan);
            await stream.WriteAsync(Encoding.UTF8.GetBytes(getBillJson));

            var billResponseJson = GetResponseJson(stream);

            var billData = JsonSerializer.Deserialize<ResponseHeader>(billResponseJson);

            if (billData?.Success == true)
            {
                var billDataResponse = ((JsonElement)billData.Data).Deserialize<CalculateBillResponse>();
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine($"The current bill is £{billDataResponse?.CurrentTotal ?? -1}");
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("An error occurred calculating the bill");
                Console.ForegroundColor = ConsoleColor.White;
            }

            if (response?.TerminateConnection == true)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("The server has terminated the connection");
                Console.ForegroundColor = ConsoleColor.White;

                stream.Close();
                return;
            }

            readingCount = 1;
        }
        else
        {
            readingCount++;
        }

    }
}

string GetMpan()
{
    const int MPAN_LENGTH = 13;
    var currentProcess = Process.GetCurrentProcess();
    var clientProcessCount = Process.GetProcesses().Count(x => x.ProcessName.Equals(currentProcess.ProcessName));
    return clientProcessCount.ToString().PadRight(MPAN_LENGTH, '0');
}

static string GetConnectionRequestJson(string mpan, int openingReading)
{
    var connectionData = new MeterConnectionRequest { Mpan = mpan, MeterReading = openingReading };
    var connectionRequest = new RequestHeader<MeterConnectionRequest>(connectionData);
    return JsonSerializer.Serialize(connectionRequest);
}

static string GetMeterReadingJson(string mpan, int meterReading)
{
    var meterReadingData = new MeterReadingRequest { Mpan = mpan, MeterReading = meterReading };
    var meterReadingRequest = new RequestHeader<MeterReadingRequest>(meterReadingData);

    return JsonSerializer.Serialize(meterReadingRequest);
}

static string GetGetBillJson(string mpan)
{
    var getBillData = new GetBillRequest { Mpan = mpan };
    var getBillRequest = new RequestHeader<GetBillRequest>(getBillData);

    return JsonSerializer.Serialize(getBillRequest);
}

static string GetResponseJson(NetworkStream responseStream)
{
    var input = new byte[1024];
    var readBytes = responseStream.Read(input, 0, input.Length);

    var trimmedData = new byte[readBytes];
    for (var i = 0; i < readBytes; i++)
    {
        trimmedData[i] = input[i];
    }

    return Encoding.UTF8.GetString(trimmedData, 0, trimmedData.Length);
}