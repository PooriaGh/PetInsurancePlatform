using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class Owner : Entity
{
    public static readonly Owner None = new();

    // Used by EF Core
    private Owner() : base()
    {
    }

    public string FirstName { get; private set; } = null!;

    public string LastName { get; private set; } = null!;

    public string FullName => FirstName + " " + LastName;

    public string NationalID { get; private set; } = null!;

    public DateOnly DateOfBirth { get; private set; }

    public City City { get; private set; } = null!;

    public static Owner Create(
        string firstName,
        string lastName,
        string nationalID,
        DateOnly DateOfBirth,
        City City)
    {
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
