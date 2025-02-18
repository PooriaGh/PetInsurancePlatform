using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.SharedKernel.Abstractions;
using PetInsurancePlatform.SharedKernel.Extensions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class Owner : Entity
{
    public static readonly Owner None = new();

    // Used by EF Core
    private Owner() : base()
    {
    }

    public string FirstName { get; private set; } = string.Empty;

    public string LastName { get; private set; } = string.Empty;

    public string FullName => FirstName + " " + LastName;

    public long NationalID { get; private set; }

    public DateOnly DateOfBirth { get; private set; }

    public City City { get; private set; } = City.None;

    public IReadOnlyCollection<Pet> Pets { get; } = [];

    public static Result<Owner> Create(
        string firstName,
        string lastName,
        long nationalID,
        DateOnly DateOfBirth,
        City City)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Invalid(OwnerErrors.EmptyFirstName);
        }

        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Invalid(OwnerErrors.EmptyLastName);
        }

        if (nationalID < 0)
        {
            return Result.Invalid(OwnerErrors.InvalidNationalID);
        }

        if (nationalID.GetLength() != 10)
        {
            return Result.Invalid(OwnerErrors.WrongLengthNationalID);
        }

        return new Owner
        {
            FirstName = firstName,
            LastName = lastName,
            NationalID = nationalID,
            DateOfBirth = DateOfBirth,
            City = City,
        };
    }
}
