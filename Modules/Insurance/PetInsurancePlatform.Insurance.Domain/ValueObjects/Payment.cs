using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Enums;
using PetInsurancePlatform.SharedKernel.Abstractions;
using System.Text.Json;

namespace PetInsurancePlatform.Insurance.Domain.ValueObjects;

public sealed class Payment : ValueObject
{
    public static readonly Payment None = new();

    public static readonly ValidationError Empty = new("The payment is required.");

    // Used by EF Core
    private Payment()
    {
    }

    public Money Amount { get; private set; } = Money.Zero;

    public Guid ReferenceNumber { get; private set; }

    public long ReservationNumber { get; private set; }

    public PaymentMethod Method { get; private set; }

    public PaymentStatus Status { get; private set; }

    public JsonDocument? Details { get; private set; }

    public static Result<Payment> Create(
        Money amount,
        Guid referenceNumber,
        long reservationNumber,
        PaymentMethod method,
        PaymentStatus status,
        JsonDocument? details)
    {
        if (amount is null || amount == Money.Zero)
        {
            return Result.Invalid(new ValidationError("The amount of payment is required."));
        }

        return new Payment
        {
            Amount = amount,
            ReferenceNumber = referenceNumber,
            ReservationNumber = reservationNumber,
            Method = method,
            Status = status,
            Details = details,
        };
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return ReferenceNumber;
    }
}
