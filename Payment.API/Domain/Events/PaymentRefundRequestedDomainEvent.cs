using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public record PaymentRefundRequestedDomainEvent(
    Guid PaymentId,
    decimal RefundAmount,
    string Reason) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
