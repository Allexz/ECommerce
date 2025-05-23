﻿namespace Payment.API.Domain.Exceptions;

public class PaymentDomainException : Exception
{
    public PaymentDomainException(string message)
        : base(message)
    {
        
    }
}
