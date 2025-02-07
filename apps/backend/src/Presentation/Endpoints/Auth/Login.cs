using Application.Commands.Auth.Login;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class Login : IEndpoint
{
    public sealed record Request(string Email, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/login",
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
            .WithTags(Tags.Auth)
            .WithSummary("Login user");
    }
}
