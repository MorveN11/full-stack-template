using Application.Commands.Auth.Register;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class Register : IEndpoint
{
    public sealed record Request(string Email, string FirstName, string LastName, string Password);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/register",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new RegisterUserCommand(
                        request.Email,
                        request.FirstName,
                        request.LastName,
                        request.Password
                    );

                    Result<Guid> result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Auth)
            .WithSummary("Register user");
    }
}
