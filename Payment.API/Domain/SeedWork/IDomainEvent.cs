namespace Payment.API.Domain.SeedWork;

public interface IDomainEvent
{
    DateTime OccurredOn { get; }
}
