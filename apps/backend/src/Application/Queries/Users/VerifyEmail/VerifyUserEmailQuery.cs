using Application.Abstractions.Messaging;

namespace Application.Queries.Users.VerifyEmail;

public sealed record VerifyUserEmailQuery(Guid TokenId) : IQuery<string>;
