namespace Payment.API.Domain.SeedWork;

public record PaymentCompletedDomainEvent(Guid PaymentId) : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
