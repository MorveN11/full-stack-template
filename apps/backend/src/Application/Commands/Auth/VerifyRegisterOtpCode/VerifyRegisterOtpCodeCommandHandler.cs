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
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.VerifyRegisterOtpCode;

internal sealed class VerifyRegisterOtpCodeCommandHandler(
    IApplicationDbContext context,
    IDateTimeProvider timeProvider,
    ITokenProvider tokenProvider,
    IUserContext userContext,
    ICacheService cacheService
) : ICommandHandler<VerifyRegisterOtpCodeCommand, VerifyRegisterOtpCodeResponse>
{
    public async Task<Result<VerifyRegisterOtpCodeResponse>> Handle(
        VerifyRegisterOtpCodeCommand command,
        CancellationToken cancellationToken
    )
    {
        User? user = await context
            .Users.Include(u => u.Roles)
            .Include(u => u.Profile)
            .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<VerifyRegisterOtpCodeResponse>(
                UserErrors.NotFound(command.Email)
            );
        }

        if (user.Status != UserStatus.Pending || user.EmailVerified)
        {
            return Result.Failure<VerifyRegisterOtpCodeResponse>(UserErrors.EmailAlreadyVerified);
        }

        OtpCode? otpCode = await context.OtpCodes.SingleOrDefaultAsync(
            o =>
                o.UserId == user.Id
                && !o.Used
                && !o.Verified
                && o.Type == OtpCodeType.Register
                && o.ExpiredOnUtc > timeProvider.UtcNow
                && o.Code == command.OtpCode,
            cancellationToken
        );

        if (otpCode is null)
        {
            return Result.Failure<VerifyRegisterOtpCodeResponse>(OtpCodeErrors.InvalidOtpCode);
        }

        otpCode.Used = true;
        otpCode.Verified = true;

        user.EmailVerified = true;
        user.Status = UserStatus.Active;

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

        context.OtpCodes.Update(otpCode);

        context.Users.Update(user);

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        await cacheService.EvictByTagAsync(Tags.Users, cancellationToken);

        return new VerifyRegisterOtpCodeResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PictureUrl = user.Profile?.PictureUrl,
            Roles = user.Roles.Select(r => r.Name).ToList(),
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
        };
    }
}
