using Application.Commands.Auth.ResetPassword;
using Application.Commands.Users.UpdateProfilePhoto;
using Domain.Identifiers;
using MediatR;
using Presentation.Extensions;
using Presentation.Infrastructure;
using SharedKernel.Results;

namespace Presentation.Endpoints.Users;

internal sealed class UpdateProfilePhoto : IEndpoint
{
    public sealed record Request(Guid UserId, Uri ProfilePhotoUri);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch(
                "users/profile-photo",
                async (Request request, ISender sender, CancellationToken cancellationToken) =>
                {
                    var command = new UpdateUserProfilePhotoCommand(
                        request.UserId,
                        request.ProfilePhotoUri
                    );

                    Result result = await sender.Send(command, cancellationToken);

                    return result.Match(Results.NoContent, CustomResults.Problem);
                }
            )
            .RequireAuthorization()
            .WithTags(Tags.Users)
            .WithSummary("Update profile photo");
    }
}
