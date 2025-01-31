using Application;
using Application.Queries.Users.GetById;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class GetById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "users/{userId:guid}",
                async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new GetUserByIdQuery(userId);

                    Result<UserResponse> result = await sender.Send(query, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Users)
            .RequireAuthorization()
            .AddCacheWithAuthorization(Tags.Users)
            .WithSummary("Get user by id");
    }
}
