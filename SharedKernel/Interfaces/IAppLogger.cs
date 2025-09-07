namespace SharedKernel.Interfaces;

public interface IAppLogger<T>
{
    void LogInformation(string message);
    void LogInformation(string message, string correlationId);
    void LogInformation(string message, params object[] args);
    void LogWarning(string message);
    void LogError(string message, Exception? ex = null);
    void SetCorrelationId(string correlationId);
}
