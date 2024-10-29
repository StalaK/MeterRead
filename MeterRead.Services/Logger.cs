namespace MeterRead.Services;
public static class Logger
{
    public static void LogError(string message)
    {
        var currentColour = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ForegroundColor = currentColour;
    }

    public static void LogWarning(string message)
    {
        var currentColour = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(message);
        Console.ForegroundColor = currentColour;
    }

    public static void LogInformation(string message)
    {
        var currentColour = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Console.WriteLine(message);
        Console.ForegroundColor = currentColour;
    }
}
