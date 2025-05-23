using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public record PaymentFraudulentDomainEvent(
    Guid PaymentId,
    decimal Amount) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
