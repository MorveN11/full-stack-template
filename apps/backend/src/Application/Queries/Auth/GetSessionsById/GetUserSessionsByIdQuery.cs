using Application.Abstractions.Messaging;

namespace Application.Queries.Auth.GetSessionsById;

public sealed record GetUserSessionsByIdQuery(Guid UserId) : IQuery<List<SessionResponse>>;
