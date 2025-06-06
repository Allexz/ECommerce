﻿using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Events;

public record PaymentFraudulentDomainEvent(
    Guid PaymentId,
    string reason) : IDomainEvent
{
    public DateTime OccurredOn => DateTime.UtcNow;
}
