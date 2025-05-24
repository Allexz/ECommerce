using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public record PaymentRejectedDomainEvent(Guid Id, string reason)
    : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
 