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
    public int Version { get; private set; }

    // Construtor privado para encapsulamento
    private Payment(
        Guid orderId,
        decimal amount,
        PaymentMethod paymentMethod,
        CreditCardInfo cardInfo = null)
    {
        if (orderId == Guid.Empty)
            throw new PaymentDomainException("OrderId cannot be empty");

        if (amount <= 0)
            throw new PaymentDomainException("Amount must be positive");

        if (paymentMethod == null)
            throw new PaymentDomainException("Payment method cannot be null");

        if (paymentMethod == PaymentMethod.CreditCard && cardInfo == null)
            throw new PaymentDomainException("Card info is required for credit card payments");

        
        OrderId = orderId;
        Amount = amount;
        this._paymentMethod = paymentMethod;
        _status = PaymentStatus.Pending;
        _createdAt = DateTime.UtcNow;
        _cardInfo = cardInfo;
        Version = 0;

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

    public void Refund(string reason)
    {
        if (!_status.IsCompleted)
            throw new PaymentDomainException("Only completed payments can be refunded");

        if (string.IsNullOrWhiteSpace(reason))
            throw new PaymentDomainException("Reason for refund cannot be empty");

        _status = PaymentStatus.Refunded;
        AddDomainEvent(new PaymentRefundedDomainEvent(Id,Amount,DateTime.UtcNow,reason));
    }

    public void RequestRefund(string reason, decimal? amount = null)
    {
        if (_status != PaymentStatus.Completed)
            throw new PaymentDomainException("Só pode reembolsar pagamentos completos");

        if (string.IsNullOrWhiteSpace(reason))
            throw new PaymentDomainException("Motivo é obrigatório");

        var refundAmount = amount ?? Amount;  // Usa valor total se não especificado

        if (refundAmount > Amount)
            throw new PaymentDomainException("Valor não pode exceder o pago");

        _status = PaymentStatus.RefundPending;

        AddDomainEvent(new PaymentRefundRequestedDomainEvent(
            Id,
            refundAmount,
            reason));
    }

    // Exemplo de método faltante:
    public void Cancel(string reason)
    {
        if (_status != PaymentStatus.Pending)
            throw new PaymentDomainException("Only pending payments can be canceled");

        _status = PaymentStatus.Cancelled;
        AddDomainEvent(new PaymentCancelledDomainEvent(Id, reason));
    }

    public void MarkAsFraud(string reason)
    {
        if (_status != PaymentStatus.Pending)
            throw new PaymentDomainException("Only pending payments can be marked as fraudulent");
        _status = PaymentStatus.Fraudulent;

        AddDomainEvent(new PaymentFraudulentDomainEvent(Id, reason));
    }

    public void Complete()
    {
        if (_status != PaymentStatus.Pending)
            throw new PaymentDomainException("Only pending payments can be completed");
        _status = PaymentStatus.Completed;
        AddDomainEvent(new PaymentCompletedDomainEvent(Id));
    }

    public void Rejected(string reason)
    {
        if (_status != PaymentStatus.Pending)
            throw new PaymentDomainException("Only pending payments can be rejected");
        _status = PaymentStatus.Rejected;
        AddDomainEvent(new PaymentRejectedDomainEvent(Id,reason));
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

    public void UpdateVersion(int newVersion)
    {
        Version = newVersion;
        // Adiciona lógica de atualização de versão se necessário
        //    public async Task UpdateAsync(Payment payment)
        //{
        //    var affectedRows = await _dbContext.Payments
        //        .Where(p => p.Id == payment.Id && p.Version == payment.Version)
        //        .ExecuteUpdateAsync(setters => setters
        //            .SetProperty(p => p.Status, payment.Status)
        //            .SetProperty(p => p.Version, payment.Version + 1));

        //    if (affectedRows == 0)
        //        throw new ConcurrencyException();
        //}
    }
}
