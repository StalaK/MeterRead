namespace MeterRead.Services;

public enum InvoicePeriod
{
    Daily,
    Weekly,
    Monthly
}

public enum RequestType
{
    MeterConnected,
    MeterRead,
    GetBill
}