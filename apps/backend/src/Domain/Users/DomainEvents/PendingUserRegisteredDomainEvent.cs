using SharedKernel.Domain;

namespace Domain.Users.DomainEvents;

public sealed record PendingUserRegisteredDomainEvent(Guid UserId, string UserEmail) : IDomainEvent;
