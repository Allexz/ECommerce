using Payment.API.Domain.Enums;
using Payment.API.Domain.Events;
using Payment.API.Domain.Exceptions;
using Payment.API.Domain.Interfaces;
using Payment.API.Domain.SeedWork;
using Payment.API.Domain.ValueObjects;

namespace Payment.API.Domain.Entities;

// Payment.API/Domain/Aggregates/PaymentAggregate.cs
public class Payment : AggregateRoot
{
    // Estado interno
    private PaymentMethod _paymentMethod;
    private PaymentStatus _status;
    private DateTime _createdAt;
    private CreditCardInfo _cardInfo; // Nullable para outros métodos

    // Propriedades públicas controladas
    public Guid OrderId { get; private set; }
    public decimal Amount { get; private set; }
    public string Method => _paymentMethod.Name;
    public string Status => _status.ToString();
    public string LastFourDigits => _cardInfo?.LastFourDigits ?? string.Empty;

    // Construtor privado para encapsulamento
    private Payment(
        Guid orderId,
        decimal amount,
        PaymentMethod paymentMethod,
        CreditCardInfo cardInfo = null)
    {
        OrderId = orderId;
        Amount = amount;
        this._paymentMethod = paymentMethod;
        _status = PaymentStatus.Pending;
        _createdAt = DateTime.UtcNow;
        _cardInfo = cardInfo;

        AddDomainEvent(new PaymentCreatedDomainEvent(this.Id, orderId, amount, paymentMethod, _createdAt));
    }

    // Factory Methods
    public static Payment CreateCreditCardPayment(
        Guid orderId,
        decimal amount,
        CreditCardInfo cardInfo)
    {
        // Validação invariante
        if (amount <= 0)
            throw new PaymentDomainException("Amount must be positive");

        return new Payment(
            orderId,
            amount,
            PaymentMethod.CreditCard,
            cardInfo);
    }

    public static Payment CreatePixPayment(Guid orderId, decimal amount)
    {
        return new Payment(
            orderId,
            amount,
            PaymentMethod.Pix);
    }

    // Comportamentos
    public void ProcessPayment(IPaymentProcessor processor)
    {
        // Pattern: Double Dispatch
        _status = processor.Process(this);

        if (_status.IsCompleted)
            AddDomainEvent(new PaymentCompletedDomainEvent(this.Id));
    }

    public void Refund()
    {
        if (!_status.IsCompleted)
            throw new PaymentDomainException("Only completed payments can be refunded");

        _status = PaymentStatus.Refunded;
        AddDomainEvent(new PaymentRefundedDomainEvent(Id, Amount));
    }

    // Exemplo de método faltante:
    public void Cancel(string reason)
    {
        if (_status != PaymentStatus.Pending)
            throw new PaymentDomainException("Only pending payments can be canceled");

        _status = PaymentStatus.Cancelled;
        AddDomainEvent(new PaymentCancelledDomainEvent(Id, reason));
    }

    public void MarkAsFraudulent()
    {
        if (_status != PaymentStatus.Pending)
            throw new PaymentDomainException("Only pending payments can be marked as fraudulent");
        _status = PaymentStatus.Fraudulent;

        AddDomainEvent(new PaymentFraudulentDomainEvent(Id, Amount));
    }

    // Métodos internos para o repositório
    internal void UpdateStatus(PaymentStatus newStatus) => _status = newStatus;

    protected override void When(IDomainEvent @event)
    {
        switch (@event)
        {
            case PaymentCreatedDomainEvent e:
                Id = e.PaymentId;
                break;
            case PaymentCompletedDomainEvent e:
                // Lógica de atualização se necessário
                break;
        }
    }
}
