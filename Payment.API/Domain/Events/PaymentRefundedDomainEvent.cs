using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public record PaymentRefundedDomainEvent(
    Guid PaymentId,
    decimal RefundAmount,
    DateTime RefundDate,
    string Reason) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
