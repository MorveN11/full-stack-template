using Application.Abstractions.Data;
using Application.Abstractions.Utilities;
using Domain.Tokens;
using Domain.Users;
using FluentEmail.Core;
using MediatR;
using SharedKernel.Time;

namespace Application.Commands.Users.Register;

internal sealed class UserRegisteredDomainEventHandler(
    IApplicationDbContext context,
    IFluentEmail fluentEmail,
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

        await fluentEmail
            .To(notification.UserEmail)
            .Subject("Email Verification")
            .Body(
                $"To verify your email address <a href='{verificationLink}'>click here</a>",
                isHtml: true
            )
            .SendAsync();
    }
}
