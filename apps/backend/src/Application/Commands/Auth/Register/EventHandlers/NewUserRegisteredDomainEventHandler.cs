using Application.Abstractions.Data;
using Application.Abstractions.Services;
using Domain.Enums;
using Domain.OtpCodes;
using Domain.Users.DomainEvents;
using MediatR;
using SharedKernel.Time;

namespace Application.Commands.Auth.Register.EventHandlers;

internal sealed class NewUserRegisteredDomainEventHandler(
    IApplicationDbContext context,
    IEmailService emailService,
    IDateTimeProvider timeProvider
) : INotificationHandler<NewUserRegisteredDomainEvent>
{
    public async Task Handle(
        NewUserRegisteredDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        string otpCode = OtpCode.GenerateCode();

        var otpCodeEntity = new OtpCode
        {
            UserId = notification.UserId,
            Code = otpCode,
            Type = OtpCodeType.Register,
            CreatedOnUtc = timeProvider.UtcNow,
            ExpiredOnUtc = timeProvider.UtcNow.AddMinutes(15),
        };

        context.OtpCodes.Add(otpCodeEntity);

        await context.SaveChangesAsync(cancellationToken);

        string emailTemplate = await File.ReadAllTextAsync(
            "../Domain/EmailTemplates/OtpCodeEmail.html",
            cancellationToken
        );

        emailTemplate = emailTemplate.Replace("{{otpCode}}", otpCode);

        await emailService.SendEmailAsync(
            notification.UserEmail,
            $"{otpCode} is your Verification Code",
            emailTemplate,
            isHtml: true
        );
    }
}
