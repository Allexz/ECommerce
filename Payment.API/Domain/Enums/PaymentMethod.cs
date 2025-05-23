// Payment.API/Domain/Enumerations/PaymentMethod.cs
using Payment.API.Domain.Exceptions;
using Payment.API.Domain.SeedWork;

namespace Payment.API.Domain.Enums;
public class PaymentMethod : Enumeration
{
    public static PaymentMethod CreditCard = new(1, "CreditCard");
    public static PaymentMethod Pix = new(2, "Pix");
    public static PaymentMethod Boleto = new(3, "Boleto");

    protected PaymentMethod(int id, string name) : base(id, name) { }

    public static PaymentMethod FromName(string name)
    {
        return name switch
        {
            "CreditCard" => CreditCard,
            "Pix" => Pix,
            "Boleto" => Boleto,
            _ => throw new PaymentDomainException($"Método de pagamento inválido: {name}")
        };
    }
}