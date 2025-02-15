using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class PetType : Entity
{
    // Used by EF Core
    private PetType() : base()
    {
    }

    public string Name { get; private set; } = null!;

    private readonly List<PetTypeDisease> _animalTypeDiseases = [];
    public IReadOnlyCollection<Disease> Diseases => _animalTypeDiseases
        .OrderBy(atd => atd.CreatedAt)
        .Select(d => d.Disease)
        .ToList();

    public static PetType Create(string name)
    {
        return new PetType
        {
            Name = name,
        };
    }

    public Result AddDisease(Disease disease)
    {
        if (_animalTypeDiseases.Any(atd => atd.Disease == disease))
        {
            return Result.Conflict("The disease is already added to the animal type.");
        }

        _animalTypeDiseases.Add(PetTypeDisease.Create(this, disease));

        return Result.Success();
    }

    public Result RemoveDisease(Disease disease)
    {
        var animalTypeDisease = _animalTypeDiseases.FirstOrDefault(atd => atd.Disease == disease);

        if (animalTypeDisease is null)
        {
            return Result.NotFound($"The animal type doesn't have a disease with ID = {disease.Id}.");
        }

        _animalTypeDiseases.Remove(animalTypeDisease);

        return Result.Success();
    }
}
