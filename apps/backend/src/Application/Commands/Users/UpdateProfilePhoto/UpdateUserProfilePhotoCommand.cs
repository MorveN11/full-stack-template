using Application.Abstractions.Messaging;

namespace Application.Commands.Users.UpdateProfilePhoto;

public sealed record UpdateUserProfilePhotoCommand(Guid UserId, Uri ProfilePhotoUri) : ICommand;
