using Application;
using Application.Queries.Users.VerifyEmail;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class VerifyEmail : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(
                "users/verify-email",
                async (Guid token, ISender sender, CancellationToken cancellationToken) =>
                {
                    var query = new VerifyUserEmailQuery(token);

                    Result<string> result = await sender.Send(query, cancellationToken);

                    return result.Match(Results.Ok, CustomResults.Problem);
                }
            )
            .WithTags(Tags.Users)
            .WithName(Names.VerifyEmail)
            .CacheOutput(b => b.NoCache())
            .WithSummary("Verify email");
    }
}
