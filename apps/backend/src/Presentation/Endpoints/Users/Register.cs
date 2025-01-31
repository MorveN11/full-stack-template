using Application;
using Application.Commands.Users.Register;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class Register : IEndpoint
{
    public sealed class Request
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
        public List<Guid> Roles { get; init; } = [];
    }

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "users/register",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new RegisterUserCommand(
                        request.Email,
                        request.Password,
                        request.Roles
                    );

                    Result<Guid> result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Users)
            .WithSummary("Register user");
    }
}
