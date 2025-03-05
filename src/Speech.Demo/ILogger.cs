namespace Speech.Demo;

internal interface ILogger
{
    void Log();
    void Log(string message);
    void Warn(string message);
    void Error(string message);
    void Error(Exception message);
}