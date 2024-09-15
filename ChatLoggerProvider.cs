using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

public class ChatLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, ChatLogger> _loggers = new ConcurrentDictionary<string, ChatLogger>();
    public LogLevel LogLevel { get; }
    private readonly IHubContext<LogHub> _hubContext;

    public ChatLoggerProvider(LogLevel logLevel, IHubContext<LogHub> hubContext)
    {
        LogLevel = logLevel;
        _hubContext = hubContext;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggers.GetOrAdd(categoryName, name => new ChatLogger(name, this, _hubContext));
    }

    public void Dispose()
    {
        _loggers.Clear();
    }
}
