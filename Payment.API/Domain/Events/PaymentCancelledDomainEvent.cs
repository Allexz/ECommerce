using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public sealed record PaymentCancelledDomainEvent(
    Guid paymentId, 
    string reason): IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
