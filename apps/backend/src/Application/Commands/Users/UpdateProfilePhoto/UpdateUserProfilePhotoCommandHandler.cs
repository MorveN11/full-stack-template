using Application.Abstractions.Authorization;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Authorization;
using Domain.Entities.Auth.UserProfiles;
using Domain.Entities.Auth.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Errors;
using SharedKernel.Results;
using SharedKernel.Time;

namespace Application.Commands.Users.UpdateProfilePhoto;

internal sealed class UpdateUserProfilePhotoCommandHandler(
    IApplicationDbContext context,
    IAuthorizationHandler authorizationHandler,
    IDateTimeProvider timeProvider
) : ICommandHandler<UpdateUserProfilePhotoCommand>
{
    public async Task<Result> Handle(
        UpdateUserProfilePhotoCommand command,
        CancellationToken cancellationToken
    )
    {
        Result authorizationResult = await authorizationHandler.HandleAsync(
            command.UserId,
            new PermissionAccess(Resources.Users, CrudOperations.Update),
            cancellationToken
        );

        if (authorizationResult.IsFailure)
        {
            return Result.Failure(authorizationResult.Error);
        }

        User? user = await context
            .Users.Include(u => u.Profile)
            .SingleOrDefaultAsync(u => u.Id == command.UserId, cancellationToken);

        if (!User.IsValidUser(user, command.UserId, out Error? error))
        {
            return Result.Failure(error!);
        }

        if (user!.Profile is null)
        {
            user.Profile = new UserProfile
            {
                PictureUrl = command.ProfilePhotoUri,
                UserId = user.Id,
                CreatedOnUtc = timeProvider.UtcNow,
            };

            context.UserProfiles.Add(user.Profile);
        }
        else
        {
            user.Profile.PictureUrl = command.ProfilePhotoUri;

            context.UserProfiles.Update(user.Profile);
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
