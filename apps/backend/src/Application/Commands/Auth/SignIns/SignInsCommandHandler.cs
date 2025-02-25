using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
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
            .SingleOrDefaultAsync(x => x.Email == request.Email, cancellationToken);

        if (user is null)
        {
            return Result.Failure(UserErrors.NotFound(request.Email));
        }

        return Result.Success();
    }
}
