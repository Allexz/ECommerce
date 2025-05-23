using Payment.API.Domain.Enums;
using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

// Payment.API/Domain/Events/PaymentCreatedDomainEvent.cs
public sealed record PaymentCreatedDomainEvent(
    Guid PaymentId,
    Guid OrderId,
    decimal Amount,
    PaymentMethod Method,
    DateTime CreatedAt) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
