using Application.Abstractions.Messaging;

namespace Application.Queries.Users.GetById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;
