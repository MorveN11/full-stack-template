using Application.Commands.Auth.VerifyRegisterOtpCode;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class VerifyRegisterOtpCode : IEndpoint
{
    public sealed record Request(string Email, string OtpCode);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/verify-register-otp-code",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new VerifyRegisterOtpCodeCommand(request.Email, request.OtpCode);

                    Result<VerifyRegisterOtpCodeResponse> result = await sender.Send(
                        query,
                        cancellationToken
                    );

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Auth)
            .WithSummary("Verify Register OTP code");
    }
}
