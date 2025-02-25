using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Entities.Auth.Users;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedKernel.Results;

namespace Application.Commands.Auth.SignIns;

internal sealed class SignInsCommandHandler(IApplicationDbContext context)
    : ICommandHandler<SignInsCommand>
{
    public async Task<Result> Handle(SignInsCommand request, CancellationToken cancellationToken)
    {
        User? user = await context
            .Users.AsNoTracking()
            .SingleOrDefaultAsync(
                x => x.Email == request.Email && x.EmailVerified && x.Status == UserStatus.Active,
                cancellationToken
            );

        return user is null ? Result.Failure(UserErrors.NotFound(request.Email)) : Result.Success();
    }
}
