using Payment.API.Domain.Enums;
 
namespace Payment.API.Domain.Interfaces;

// Payment.API/Domain/Interfaces/IPaymentProcessor.cs
public interface IPaymentProcessor
{
    PaymentStatus Process(Payment.API.Domain.Entities.Payment payment);
}