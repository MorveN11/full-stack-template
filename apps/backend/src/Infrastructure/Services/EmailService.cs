using Domain.Services;
using FluentEmail.Core;

namespace Infrastructure.Services;

internal sealed class EmailService(IFluentEmail fluentEmail) : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        await fluentEmail.To(to).Subject(subject).Body(body, isHtml: isHtml).SendAsync();
    }
}
