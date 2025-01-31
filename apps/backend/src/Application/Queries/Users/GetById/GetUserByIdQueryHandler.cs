using Application.Abstractions.Authentication;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Queries.Users.GetById;

internal sealed class GetUserByIdQueryHandler(
    IApplicationDbContext context,
    IUserContext userContext
) : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        if (query.UserId != userContext.UserId)
        {
            return Result.Failure<UserResponse>(UserErrors.Unauthorized);
        }

        User? user = await context
            .Users.AsNoTracking()
            .Where(u => u.Id == query.UserId)
            .SingleOrDefaultAsync(cancellationToken);

        if (user is null || !user.IsActive)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound(query.UserId));
        }

        return new UserResponse { Id = user.Id, Email = user.Email };
    }
}
