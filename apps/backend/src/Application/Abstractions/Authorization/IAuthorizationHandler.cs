using Domain.Authorization;
using SharedKernel.Results;

namespace Application.Abstractions.Authorization;

public interface IAuthorizationHandler
{
    Task<Result> HandleAsync(
        PermissionAccess permissionAccess,
        CancellationToken cancellationToken = default
    );

    Task<Result> HandleAsync(
        Guid ownerId,
        PermissionAccess permissionAccess,
        CancellationToken cancellationToken = default
    );
}
