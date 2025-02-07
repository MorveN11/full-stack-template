using Application.Abstractions.Authentication;
using Application.Abstractions.Authorization;
using Domain.Authorization;
using Domain.Users;
using SharedKernel.Results;

namespace Infrastructure.Authorization;

internal sealed class AuthorizationHandler(
    IUserContext userContext,
    IPermissionProvider permissionProvider
) : IAuthorizationHandler
{
    public async Task<Result> HandleAsync(
        PermissionAccess permissionAccess,
        CancellationToken cancellationToken = default
    )
    {
        HashSet<string> permissions = await permissionProvider.GetForUserIdAsync(
            userContext.UserId,
            cancellationToken
        );

        return permissions.Contains(permissionAccess.Key)
            ? Result.Success()
            : Result.Failure(UserErrors.Forbidden);
    }

    public async Task<Result> HandleAsync(
        Guid ownerId,
        PermissionAccess permissionAccess,
        CancellationToken cancellationToken = default
    )
    {
        Result handle = await HandleAsync(permissionAccess, cancellationToken);

        if (handle.IsSuccess)
        {
            return Result.Success();
        }

        HashSet<string> permissions = await permissionProvider.GetForUserIdAsync(
            ownerId,
            cancellationToken
        );

        if (permissions.Contains(permissionAccess.SelfAccessKey) && ownerId == userContext.UserId)
        {
            return Result.Success();
        }

        return Result.Failure(UserErrors.Forbidden);
    }
}
