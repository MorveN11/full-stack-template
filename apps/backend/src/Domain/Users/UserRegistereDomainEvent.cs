using SharedKernel.Domain;

namespace Domain.Users;

public sealed record UserRegisteredDomainEvent(Guid UserId, string UserEmail) : IDomainEvent;
