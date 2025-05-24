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
    public static PaymentStatus Cancelled = new(5, "Cancelled");
    public static PaymentStatus Fraudulent = new(6, "Fraudulent");
    public static PaymentStatus Rejected = new(7, "Rejected");
    public static PaymentStatus RefundPending = new(8, "RefundPending");

    public bool IsCompleted => Completed.Equals(this);

    public static PaymentStatus FromName(string name)
    {
        return name switch
        {
            "Pending" => Pending,
            "Completed" => Completed,
            "Failed" => Failed,
            "Refunded" => Refunded,
            "Cancelled" => Cancelled,
            "Fraudulent" => Fraudulent,
            "Rejected" => Rejected,
            "RefundPending" => RefundPending,
            _ => throw new PaymentDomainException($"Status de pagamento inválido: {name}")
        };
    }

    
}
