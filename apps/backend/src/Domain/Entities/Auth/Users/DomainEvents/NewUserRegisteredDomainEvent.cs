using SharedKernel.Domain;

namespace Domain.Entities.Auth.Users.DomainEvents;

public sealed record NewUserRegisteredDomainEvent(Guid UserId, string UserEmail) : IDomainEvent;
