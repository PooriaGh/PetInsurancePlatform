using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.Insurance.Domain.ValueObjects;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class PetType : Entity
{
    public static readonly PetType None = new();

    // Used by EF Core
    private PetType() : base()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public AgeRange AgeRange { get; private set; } = AgeRange.None;

    private readonly List<PetTypeDisease> _petTypeDiseases = [];
    public IReadOnlyCollection<Disease> Diseases => _petTypeDiseases
        .OrderBy(atd => atd.CreatedAt)
        .Select(d => d.Disease)
        .ToList()
        .AsReadOnly();

    public static Result<PetType> Create(
        string name,
        AgeRange ageRange)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(PetTypeErrors.EmptyName);
        }

        return new PetType
        {
            Name = name,
            AgeRange = ageRange,
        };
    }

    public Result AddDisease(Disease disease)
    {
        if (disease is null)
        {
            return Result.Invalid(DiseaseErrors.Empty);
        }

        if (_petTypeDiseases.Any(atd => atd.Disease == disease))
        {
            return Result.Conflict(PetTypeErrors.DuplicateDisease(disease.Name));
        }

        _petTypeDiseases.Add(PetTypeDisease.Create(this, disease));

        return Result.Success();
    }

    public Result RemoveDisease(Disease disease)
    {
        if (disease is null)
        {
            return Result.Invalid(DiseaseErrors.Empty);
        }

        var petTypeDisease = _petTypeDiseases.FirstOrDefault(atd => atd.Disease == disease);

        if (petTypeDisease is null)
        {
            return Result.NotFound(PetTypeErrors.NotFoundDisease(disease.Id));
        }

        _petTypeDiseases.Remove(petTypeDisease);

        return Result.Success();
    }
}
