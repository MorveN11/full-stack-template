using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Responses;
using Application.Abstractions.Services;
using Domain.Authorization;
using Domain.Identifiers;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Queries.Users.GetAll;

internal sealed class GetAllUsersQueryHandler(
    IApplicationDbContext context,
    ICacheService cacheService,
    IAuthorizationHandler authorizationHandler
) : IQueryHandler<GetAllUsersQuery, PagedList<UserResponse>>
{
    private async Task<Result<PagedList<UserResponse>>> GetAllUsersAsync(
        GetAllUsersQuery query,
        CancellationToken cancellationToken
    )
    {
        IQueryable<UserResponse> users = context
            .Users.AsNoTracking()
            .AsSplitQuery()
            .Include(u => u.Roles)
            .ThenInclude(r => r.Permissions)
            .OrderByDescending(u => u.UpdatedOnUtc)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Roles = u.Roles.Select(r => r.Name).ToList(),
                Permissions = u
                    .Roles.SelectMany(r => r.Permissions)
                    .Select(p => p.Name)
                    .ToHashSet(),
            });

        return await PagedList.CreateAsync(users, query.Page, query.PageSize, cancellationToken);
    }

    public async Task<Result<PagedList<UserResponse>>> Handle(
        GetAllUsersQuery query,
        CancellationToken cancellationToken
    )
    {
        Result authorizationResult = await authorizationHandler.HandleAsync(
            new PermissionAccess(Resources.Users, CrudOperations.Read),
            cancellationToken
        );

        if (authorizationResult.IsFailure)
        {
            return Result.Failure<PagedList<UserResponse>>(authorizationResult.Error);
        }

        return await cacheService.GetOrCreateAsync(
            $"users:{query.Page}:{query.PageSize}",
            async ct => await GetAllUsersAsync(query, ct),
            [Tags.Users],
            cancellationToken
        );
    }
}
