using Microsoft.Extensions.Logging;
using server.Application.Interfaces;

namespace server.Infrastructure.Services;

public class ConsoleEmailSender : IEmailSender
{
    private readonly ILogger<ConsoleEmailSender> _logger;

    public ConsoleEmailSender(ILogger<ConsoleEmailSender> logger) => _logger = logger;

    public Task SendAsync(string toEmail, string subject, string body, CancellationToken ct = default)
    {
        _logger.LogInformation(
            "\n========== [DEV EMAIL] ==========\nTo:      {To}\nSubject: {Subject}\n---------------------------------\n{Body}\n=================================",
            toEmail, subject, body);
        return Task.CompletedTask;
    }
}
