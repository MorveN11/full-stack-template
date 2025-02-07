using Application.Abstractions.Data;
using Application.Abstractions.Factories;
using Application.Abstractions.Services;
using Domain.Tokens;
using Domain.Users;
using MediatR;
using SharedKernel.Time;

namespace Application.Commands.Auth.Register;

internal sealed class UserRegisteredDomainEventHandler(
    IApplicationDbContext context,
    IEmailService emailService,
    IEmailVerificationLinkFactory emailVerificationLinkFactory,
    IDateTimeProvider timeProvider
) : INotificationHandler<UserRegisteredDomainEvent>
{
    public async Task Handle(
        UserRegisteredDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        var verificationToken = new EmailVerificationToken
        {
            UserId = notification.UserId,
            CreatedOnUtc = timeProvider.UtcNow,
        };

        string verificationLink = emailVerificationLinkFactory.Create(verificationToken);

        context.EmailVerificationTokens.Add(verificationToken);

        await context.SaveChangesAsync(cancellationToken);

        await emailService.SendEmailAsync(
            notification.UserEmail,
            "Email Verification",
            $"To verify your email address <a href='{verificationLink}'>click here</a>",
            isHtml: true
        );
    }
}
