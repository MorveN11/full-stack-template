using Application.Commands.Auth.VerifyForgotPasswordOtpCode;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class VerifyForgotPasswordOtpCode : IEndpoint
{
    public sealed record Request(string Email, string OtpCode);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/verify-forgot-password-otp-code",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new VerifyForgotPasswordOtpCodeCommand(
                        request.Email,
                        request.OtpCode
                    );

                    Result result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Auth)
            .WithSummary("Verify Forgot Password OTP code");
    }
}
