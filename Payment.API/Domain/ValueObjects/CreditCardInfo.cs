using Payment.API.Domain.Exceptions;
using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.ValueObjects;

// Payment.API/Domain/ValueObjects/CreditCardInfo.cs
public sealed class CreditCardInfo : ValueObject
{
    public string TokenizedCard { get; } // Armazena token do gateway
    public string LastFourDigits { get; }
    public string Brand { get; }
    public int ExpiryMonth { get; }
    public int ExpiryYear { get; }

    public CreditCardInfo(
        string tokenizedCard,
        string lastFourDigits,
        string brand,
        int expiryMonth,
        int expiryYear)
    {
        // Validações rigorosas
        if (string.IsNullOrWhiteSpace(tokenizedCard))
            throw new PaymentDomainException("Token inválido");

        if (lastFourDigits?.Length != 4 || !lastFourDigits.All(char.IsDigit))
            throw new PaymentDomainException("Últimos 4 dígitos inválidos");

        TokenizedCard = tokenizedCard;
        LastFourDigits = lastFourDigits;
        Brand = brand;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return TokenizedCard;
        yield return LastFourDigits;
        yield return Brand;
        yield return ExpiryMonth;
        yield return ExpiryYear;
    }
}