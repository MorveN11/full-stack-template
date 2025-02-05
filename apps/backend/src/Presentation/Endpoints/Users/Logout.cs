using Application;
using Application.Commands.Users.Logout;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class Logout : IEndpoint
{
    public sealed class Request
    {
        public required Guid UserId { get; init; }
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        _ = app.MapPost(
                "users/logout",
                static async (
                    Request request,
                    ISender sender,
                    CancellationToken cancellationToken
                ) =>
                {
                    var command = new LogoutUserCommand(request.UserId);

                    Result result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Users)
            .RequireAuthorization()
            .WithSummary("Logout user");
    }
}
