using SharedKernel.Domain;

namespace Domain.Entities.Auth.Users.DomainEvents;

public sealed record PendingUserRegisteredDomainEvent(Guid UserId, string UserEmail) : IDomainEvent;
