using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;

namespace TripSplit.Web.Services;

public sealed class NoOpEmailSender : IEmailSender
{
    private readonly ILogger<NoOpEmailSender> _logger;
    public NoOpEmailSender(ILogger<NoOpEmailSender> logger) => _logger = logger;

    public Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        _logger.LogInformation("DEV NoOpEmailSender: to={Email}, subject={Subject}, bodyLen={Len}",
            email, subject, htmlMessage?.Length ?? 0);
        return Task.CompletedTask;
    }
}
