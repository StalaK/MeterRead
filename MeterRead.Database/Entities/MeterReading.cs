namespace MeterRead.Database.Entities;

public sealed class MeterReading
{
    public required int Id { get; set; }

    public required int ClientId { get; set; }

    public required decimal Reading { get; set; }

    public required decimal Rate { get; set; }

    public required DateTime ReadingTime { get; set; }
}
