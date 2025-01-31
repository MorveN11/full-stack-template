using Application;
using Application.Commands.Users.Login;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class Login : IEndpoint
{
    public sealed class Request
    {
        public required string Email { get; init; }
        public required string Password { get; init; }
    };

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "users/login",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new LoginUserCommand(request.Email, request.Password);

                    Result<LoginUserResponse> result = await sender.Send(
                        command,
                        cancellationToken
                    );

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Users)
            .WithSummary("Login user");
    }
}
