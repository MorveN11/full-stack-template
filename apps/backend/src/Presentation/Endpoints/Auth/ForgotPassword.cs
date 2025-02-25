using Application.Commands.Auth.ForgotPassword;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Auth;

internal sealed class ForgotPassword : IEndpoint
{
    public sealed record Request(string Email);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(
                "auth/forgot-password",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new ForgotPasswordCommand(request.Email);

                    Result result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Auth)
            .WithSummary("Forgot password");
    }
}
