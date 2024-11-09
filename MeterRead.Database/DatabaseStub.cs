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
            new Client { Id = 1, Mpan = "1000000000000" },
            new Client { Id = 2, Mpan = "2000000000000" },
            new Client { Id = 3, Mpan = "3000000000000" },
            new Client { Id = 4, Mpan = "4000000000000" },
            new Client { Id = 5, Mpan = "5000000000000" },
            new Client { Id = 6, Mpan = "6000000000000" },
            new Client { Id = 7, Mpan = "7000000000000" },
            new Client { Id = 8, Mpan = "8000000000000" },
            new Client { Id = 9, Mpan = "9000000000000" },
            new Client { Id = 10, Mpan = "1000000000000" },
            new Client { Id = 11, Mpan = "1100000000000" },
            new Client { Id = 12, Mpan = "1200000000000" },
            new Client { Id = 13, Mpan = "1300000000000" },
            new Client { Id = 14, Mpan = "1400000000000" },
            new Client { Id = 15, Mpan = "1500000000000" },
            new Client { Id = 16, Mpan = "1600000000000" },
            new Client { Id = 17, Mpan = "1700000000000" },
            new Client { Id = 18, Mpan = "1800000000000" },
            new Client { Id = 19, Mpan = "1900000000000" },
            new Client { Id = 20, Mpan = "2000000000000" },
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
