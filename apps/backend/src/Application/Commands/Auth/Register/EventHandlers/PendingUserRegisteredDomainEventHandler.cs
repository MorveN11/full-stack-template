using Application.Abstractions.Data;
using Domain.Entities.Auth.OtpCodes;
using Domain.Entities.Auth.Users;
using Domain.Entities.Auth.Users.DomainEvents;
using Domain.Enums;
using Domain.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Time;

namespace Application.Commands.Auth.Register.EventHandlers;

internal sealed class PendingUserRegisteredDomainEventHandler(
    IApplicationDbContext context,
    IEmailService emailService,
    IDateTimeProvider timeProvider
) : INotificationHandler<PendingUserRegisteredDomainEvent>
{
    public async Task Handle(
        PendingUserRegisteredDomainEvent notification,
        CancellationToken cancellationToken
    )
    {
        await context
            .OtpCodes.Where(o =>
                o.UserId == notification.UserId
                && !o.Used
                && !o.Verified
                && o.Type == OtpCodeType.Register
            )
            .ExecuteUpdateAsync(o => o.SetProperty(p => p.Used, true), cancellationToken);

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
