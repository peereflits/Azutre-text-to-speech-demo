namespace Speech.Demo;

internal class Program
{
    private static readonly ILogger logger = new ConsoleLogger();

    private static async Task Main(string[] args)
    {
        var settings = new ConfigurationManager().Settings;
        var synth = new Synthesizer(settings, logger);

        Console.BackgroundColor = ConsoleColor.DarkBlue;
        Console.ForegroundColor = ConsoleColor.White;

        Log("======================================================================");
        Log("== Welcome to the Text-To-Speech demo");
        Log("======================================================================");
        Log();

        var input = args.Any() ? args[0] : string.Empty;

        while (input != "-q")
        {
            if (File.Exists(input))
            {
                var text = await File.ReadAllTextAsync(input);
                await synth.AudioAsync(text);
            }

            Log("Enter a text file to synthesize or -q to quit:");
            input = Console.ReadLine();
        }

        Log("Bye");
    }

    private static void Log()
    {
        logger.Log();
    }

    private static void Log(string msg)
    {
        logger.Log(msg);
    }
}