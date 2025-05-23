namespace Payment.API.Domain.Interfaces;

// Interface do gateway
public interface ICardTokenizer
{
    Task<string> TokenizeCardAsync(
        string cardNumber,
        string cvv,
        int expiryMonth,
        int expiryYear);
}
