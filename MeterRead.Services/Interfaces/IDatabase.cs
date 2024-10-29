namespace MeterRead.Services.Interfaces;
public interface IDatabase
{
    bool ValidClient(string mpan);

    void RecordReading(string mpan, decimal reading, decimal rate);
}
