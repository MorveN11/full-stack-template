using Application.Commands.Auth.UpdatePassword;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class UpdatePassword : IEndpoint
{
    public sealed record Request(
        Guid UserId,
        string Email,
        string NewPassword,
        string RefreshToken,
        bool SignOutEverywhere = true
    );

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(
                "auth/update-password",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new UpdatePasswordCommand(
                        request.UserId,
                        request.Email,
                        request.NewPassword,
                        request.RefreshToken,
                        request.SignOutEverywhere
                    );

                    Result result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .RequireAuthorization()
            .WithTags(Tags.Auth)
            .WithSummary("Update Password with an Authorized User");
    }
}
