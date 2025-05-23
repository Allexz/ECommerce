using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public sealed record PaymentApprovedDomainEvent(
    Guid paymentId,
    Guid orderId,
    decimal amount,
    string method) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
