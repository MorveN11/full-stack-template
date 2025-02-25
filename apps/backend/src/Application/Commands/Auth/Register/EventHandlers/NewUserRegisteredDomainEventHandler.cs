using Application.Abstractions.Data;
using Domain.Entities.Auth.OtpCodes;
using Domain.Entities.Auth.Users;
using Domain.Entities.Auth.Users.DomainEvents;
using Domain.Enums;
using Domain.Services;
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

        await User.SendOtpCodeAsync(
            emailService,
            notification.UserEmail,
            otpCode,
            cancellationToken
        );
    }
}
