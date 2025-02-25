using SharedKernel.Domain;

namespace Domain.Users.DomainEvents;

public sealed record NewUserRegisteredDomainEvent(Guid UserId, string UserEmail) : IDomainEvent;
