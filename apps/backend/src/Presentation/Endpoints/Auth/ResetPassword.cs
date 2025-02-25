using Application.Commands.Auth.ResetPassword;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class ResetPassword : IEndpoint
{
    public sealed record Request(string Email, string NewPassword, string OtpCode);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(
                "auth/reset-password",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new ResetPasswordCommand(
                        request.Email,
                        request.NewPassword,
                        request.OtpCode
                    );

                    Result<ResetPasswordResponse> result = await sender.Send(
                        command,
                        cancellationToken
                    );

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Auth)
            .WithSummary("Reset password");
    }
}
