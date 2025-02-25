using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Auth.OtpCodes;
using Domain.Entities.Auth.RefreshTokens;
using Domain.Entities.Auth.Users;
using Domain.Enums;
using Domain.Identifiers;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.ResetPassword;

internal sealed class ResetPasswordCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    IDateTimeProvider timeProvider,
    IUserContext userContext,
    ICacheService cacheService
) : ICommandHandler<ResetPasswordCommand, ResetPasswordResponse>
{
    public async Task<Result<ResetPasswordResponse>> Handle(
        ResetPasswordCommand command,
        CancellationToken cancellationToken
    )
    {
        User? user = await context
            .Users.Include(u => u.Roles)
            .Include(u => u.Profile)
            .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (!User.IsValidUser(user, command.Email, out Error? error))
        {
            return Result.Failure<ResetPasswordResponse>(error!);
        }

        OtpCode? otpCode = await context.OtpCodes.SingleOrDefaultAsync(
            o =>
                o.UserId == user!.Id
                && !o.Used
                && o.Verified
                && o.Type == OtpCodeType.ResetPassword
                && o.Code == command.OtpCode,
            cancellationToken
        );

        if (otpCode is null)
        {
            return Result.Failure<ResetPasswordResponse>(OtpCodeErrors.InvalidOtpCode);
        }

        otpCode.Used = true;

        user!.PasswordHash = passwordHasher.Hash(command.NewPassword);

        string accessToken = tokenProvider.Create(user);

        var refreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = tokenProvider.GenerateRefreshToken(),
            ExpiredOnUtc = timeProvider.UtcNow.AddDays(7),
            IpAddress = userContext.IpAddress,
            UserAgent = userContext.UserAgent,
            CreatedOnUtc = timeProvider.UtcNow,
        };

        await context
            .RefreshTokens.Where(rt => rt.UserId == user.Id)
            .ExecuteDeleteAsync(cancellationToken);

        context.OtpCodes.Update(otpCode);

        context.Users.Update(user);

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        await cacheService.EvictByTagAsync(Tags.Auth, cancellationToken);

        return new ResetPasswordResponse()
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PictureUrl = user.Profile?.PictureUrl,
            Roles = user.Roles.Select(r => r.Name).ToHashSet(),
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
        };
    }
}
