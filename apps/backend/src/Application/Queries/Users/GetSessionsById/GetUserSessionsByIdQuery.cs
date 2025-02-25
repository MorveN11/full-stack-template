using Application.Abstractions.Messaging;

namespace Application.Queries.Users.GetSessionsById;

public sealed record GetUserSessionsByIdQuery(Guid UserId) : IQuery<List<SessionResponse>>;
