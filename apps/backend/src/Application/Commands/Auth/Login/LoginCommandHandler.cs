using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Enums;
using Domain.RefreshTokens;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Auth.Login;

internal sealed class LoginCommandHandler(
    IApplicationDbContext context,
    IPasswordHasher passwordHasher,
    ITokenProvider tokenProvider,
    IDateTimeProvider timeProvider,
    IUserContext userContext
) : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken
    )
    {
        User? user = await context
            .Users.AsNoTracking()
            .Include(u => u.Roles)
            .Include(u => u.Profile)
            .SingleOrDefaultAsync(u => u.Email == command.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
        }

        bool verified = passwordHasher.Verify(command.Password, user.PasswordHash);

        if (!verified)
        {
            return Result.Failure<LoginResponse>(UserErrors.InvalidCredentials);
        }

        if (!user.EmailVerified || user.Status == UserStatus.Pending)
        {
            return Result.Failure<LoginResponse>(UserErrors.EmailNotVerified);
        }

        if (user.Status == UserStatus.Blocked)
        {
            return Result.Failure<LoginResponse>(UserErrors.Forbidden);
        }

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

        context.RefreshTokens.Add(refreshToken);

        await context.SaveChangesAsync(cancellationToken);

        var response = new LoginResponse
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

        return response;
    }
}
