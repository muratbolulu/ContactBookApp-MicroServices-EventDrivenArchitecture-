using Nest;
using SharedKernel.Interfaces;

namespace ReportService.Infrastructure.Logging;


public class ElasticsearchLogger<T> : IAppLogger<T>
{
    private readonly IElasticClient _elasticClient;
    private string _correlationId = string.Empty;

    public ElasticsearchLogger(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public void SetCorrelationId(string correlationId)
    {
        _correlationId = correlationId;
    }

    public void LogInformation(string message)
    {
        Log("Information", message);
    }

    public void LogWarning(string message)
    {
        Log("Warning", message);
    }

    public void LogError(string message, Exception? ex = null)
    {
        var fullMessage = ex != null ? $"{message} | Exception: {ex}" : message;
        Log("Error", fullMessage);
    }

    private void Log(string level, string message)
    {
        var logEntry = new
        {
            Timestamp = DateTime.UtcNow,
            Level = level,
            CorrelationId = _correlationId,
            Service = typeof(T).Name,
            Message = message
        };

        _elasticClient.IndexDocument(logEntry);
    }

    public void LogInformation(string message, string correlationId)
    {
        Log("Information", $"{message} | CorrelationId: {correlationId}");
    }

    public void LogInformation(string message, params object[] args)
    {
        var formattedMessage = string.Format(message, args);
        Log("Information", formattedMessage);
    }
}
