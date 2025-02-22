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

    private readonly List<Pet> _pets = [];
    public IReadOnlyCollection<Pet> Pets => _pets.AsReadOnly();

    private readonly List<OwnerTermsOfService> _ownerTermsOfServices = [];
    public IReadOnlyCollection<TermsOfService> TermsOfServices => _ownerTermsOfServices
        .Select(terms => terms.TermsOfService)
        .OrderByDescending(terms => terms.Version)
        .ToList()
        .AsReadOnly();

    public static Result<Owner> Create(
        string firstName,
        string lastName,
        long nationalID,
        DateOnly DateOfBirth)
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
        };
    }

    public Result<Pet> AddPet(Pet pet)
    {
        if (pet is null)
        {
            return Result.NotFound(PetErrors.NotFound());
        }

        if (pet == Pet.None)
        {
            return Result.NotFound(PetErrors.NotFound(pet.Id));
        }

        _pets.Add(pet);

        return pet;
    }

    public Result RemovePet(Guid id)
    {
        var pet = _pets.FirstOrDefault(p => p.Id == id);

        if (pet is null)
        {
            return Result.NotFound(PetErrors.NotFound(id));
        }

        pet.Delete();

        return Result.Success();
    }

    public Result AcceptTermsOfService(TermsOfService termsOfService)
    {
        if (_ownerTermsOfServices.Any(t => t == termsOfService))
        {
            return Result.Invalid(TermsOfServiceErrors.AlreadyAccepted);
        }

        var ownerTermsOfService = OwnerTermsOfService.Create(termsOfService, this);

        if (!ownerTermsOfService.IsSuccess)
        {
            return Result.Error(new ErrorList([.. ownerTermsOfService.Errors]));
        }

        _ownerTermsOfServices.Add(ownerTermsOfService.Value);

        return Result.Success();
    }
}
