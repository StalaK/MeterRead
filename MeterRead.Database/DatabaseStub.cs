using MeterRead.Database.Entities;
using MeterRead.Services.Interfaces;

namespace MeterRead.Database;

public sealed class DatabaseStub : IDatabase
{
    public List<Client> Clients { get; }

    public List<MeterReading> MeterReadings { get; }

    public DatabaseStub()
    {
        Clients = [
            new Client { Id = 1, Mpan = "1111110000000" },
            new Client { Id = 2, Mpan = "2222220000000" },
            new Client { Id = 3, Mpan = "3333330000000" },
            new Client { Id = 4, Mpan = "4444440000000" },
            new Client { Id = 5, Mpan = "5555550000000" },
            new Client { Id = 6, Mpan = "6666660000000" },
            new Client { Id = 7, Mpan = "7777770000000" },
            new Client { Id = 8, Mpan = "8888880000000" },
            new Client { Id = 9, Mpan = "9999990000000" },
            new Client { Id = 10, Mpan = "1010100000000" },
            new Client { Id = 11, Mpan = "1111110000000" },
            new Client { Id = 12, Mpan = "1212120000000" },
            new Client { Id = 13, Mpan = "1313130000000" },
            new Client { Id = 14, Mpan = "1414140000000" },
            new Client { Id = 15, Mpan = "1515150000000" },
            new Client { Id = 16, Mpan = "1616160000000" },
            new Client { Id = 17, Mpan = "1717170000000" },
            new Client { Id = 18, Mpan = "1818180000000" },
            new Client { Id = 19, Mpan = "1919190000000" },
            new Client { Id = 20, Mpan = "2020200000000" },
        ];

        MeterReadings = [];
    }

    public bool ValidClient(string mpan) => Clients.Any(c => c.Mpan.Equals(mpan, StringComparison.InvariantCultureIgnoreCase));

    public void RecordReading(string mpan, decimal reading, decimal rate)
    {
        var client = Clients.FirstOrDefault(c => c.Mpan.Equals(mpan, StringComparison.InvariantCultureIgnoreCase))
            ?? throw new Exception($"{mpan} is not a valid client MPAN");

        MeterReadings.Add(new()
        {
            Id = MeterReadings.Count + 1,
            ClientId = client.Id,
            Reading = reading,
            Rate = rate,
            ReadingTime = DateTime.UtcNow
        });
    }
}
