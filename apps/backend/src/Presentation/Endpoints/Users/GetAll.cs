using Application.Abstractions.Responses;
using Application.Queries.Users.GetAll;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class GetAll : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        _ = app.MapGet(
                "users",
                static async (
                    ISender sender,
                    CancellationToken cancellationToken,
                    int page = 1,
                    int pageSize = 10
                ) =>
                {
                    var query = new GetAllUsersQuery(page, pageSize);

                    Result<PagedList<UserResponse>> result = await sender.Send(
                        query,
                        cancellationToken
                    );

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .RequireAuthorization()
            .WithTags(Tags.Users)
            .WithSummary("Get all users with pagination");
    }
}
