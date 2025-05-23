using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public record PaymentRefundedDomainEvent
    (
    Guid PaymentId,
    decimal Amount) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
