using Application.Commands.Auth.RegisterAdmin;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class RegisterAdmin : IEndpoint
{
    public sealed record Request(
        string Email,
        string FirstName,
        string LastName,
        string Password,
        List<string> Roles
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/register-admin",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new RegisterAdminUserCommand(
                        request.Email,
                        request.FirstName,
                        request.LastName,
                        request.Password,
                        request.Roles
                    );

                    Result<Guid> result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .RequireAuthorization()
            .WithTags(Tags.Auth)
            .WithSummary("Register user - Only for Admins");
    }
}
