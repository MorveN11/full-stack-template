using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Abstractions.Responses;
using SharedKernel.Results;

namespace Application.Queries.Users.GetAll;

internal sealed class GetAllUsersQueryHandler(IApplicationDbContext context)
    : IQueryHandler<GetAllUsersQuery, PagedList<UserResponse>>
{
    public async Task<Result<PagedList<UserResponse>>> Handle(
        GetAllUsersQuery query,
        CancellationToken cancellationToken
    )
    {
        IQueryable<UserResponse> users = context
            .Users.Where(u => u.IsActive)
            .OrderByDescending(u => u.UpdatedAt)
            .Select(u => new UserResponse { Id = u.Id, Email = u.Email });

        return await PagedList.CreateAsync(users, query.Page, query.PageSize);
    }
}
