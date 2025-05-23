using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public record PaymentCompletedDomainEvent(Guid PaymentId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
