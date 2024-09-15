using Microsoft.AspNetCore.SignalR;

public class ChatLogger : ILogger
{
    private readonly string _name;
    private readonly ChatLoggerProvider _provider;
    private readonly IHubContext<LogHub> _hubContext;

    public ChatLogger(string name, ChatLoggerProvider provider, IHubContext<LogHub> hubContext)
    {
        _name = name;
        _provider = provider;
        _hubContext = hubContext;
    }

    public IDisposable BeginScope<TState>(TState state) => null;

    public bool IsEnabled(LogLevel logLevel) => logLevel >= _provider.LogLevel;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }
        // string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss
        string timestamp = DateTime.Now.ToString("HH:mm:ss");

        //var logRecord = $"{timestamp} [{logLevel}] {_name}\n\t{formatter(state, exception)}";
        var logRecord = $"{timestamp} {formatter(state, exception)}";
        // Send log to SignalR hub
        Task.Run(() => _hubContext.Clients.All.SendAsync("ReceiveLog", logRecord));
    }
}
