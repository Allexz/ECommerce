using Payment.API.Domain.Exceptions;
using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Enums;

public class PaymentStatus : Enumeration
{
    public PaymentStatus(int id, string name) : base(id, name) { }
 
    public static PaymentStatus Pending = new(1, "Pending");
    public static PaymentStatus Completed = new(2, "Completed");
    public static PaymentStatus Failed = new(3, "Failed");
    public static PaymentStatus Refunded = new(4, "Refunded");

    public bool IsCompleted => Completed.Equals(this);

    public static PaymentStatus FromName(string name)
    {
        return name switch
        {
            "Pending" => Pending,
            "Completed" => Completed,
            "Failed" => Failed,
            "Refunded" => Refunded,
            _ => throw new PaymentDomainException($"Status de pagamento inválido: {name}")
        };
    }

    
}
