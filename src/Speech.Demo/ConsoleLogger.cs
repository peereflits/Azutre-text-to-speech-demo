namespace Speech.Demo;

internal class ConsoleLogger : ILogger
{
    public void Log()
    {
        Console.WriteLine();
    }

    public void Log(string message)
    {
        Console.WriteLine(message);
    }

    public void Warn(string message)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void Error(string message)
    {
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void Error(Exception ex)
    {
        var msg = $"An {ex.GetType().Name} has occurred in {ex.Source}." +
                  $"{ex.Message}";

        var inner = ex.InnerException;
        var space = "  ";
        while (inner != null)
        {
            msg += $"\r\n{space}- Inner {ex.GetType().Name}: {inner.Message}";
            inner = inner.InnerException;
            space += "  ";
        }

        Error($"{msg}\r\nStacktrace:\r\nex.StackTrace");
    }
}