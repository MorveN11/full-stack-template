using Application.Queries.Users.GetSessionsById;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class GetSessionsById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "users/{userId:guid}/sessions",
                async (Guid userId, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new GetUserSessionsByIdQuery(userId);

                    Result<List<SessionResponse>> result = await sender.Send(
                        query,
                        cancellationToken
                    );

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .RequireAuthorization()
            .WithTags(Tags.Users)
            .WithSummary("Get user sessions by id");
    }
}
