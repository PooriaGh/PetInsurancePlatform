using Ardalis.Result;
using PetInsurancePlatform.Animals.Domain.Diseases;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.AnimalTypes;

public sealed class AnimalType(Guid id) : Entity(id)
{
    public string Name { get; private set; } = null!;

    private readonly List<Animal> _animals = [];
    public IReadOnlyCollection<Animal> Animals => _animals.AsReadOnly();

    private readonly List<AnimalTypeDisease> _animalTypeDiseases = [];
    public IReadOnlyCollection<Disease> Diseases => _animalTypeDiseases
        .OrderBy(atd => atd.CreatedAt)
        .Select(d => d.Disease)
        .ToList();

    public static AnimalType Create(string name)
    {
        return new AnimalType(Guid.NewGuid())
        {
            Name = name,
        };
    }

    public Result AddAnimal(Animal animal)
    {
        if (_animals.Any(a => a.Id == animal.Id))
        {
            return Result.Conflict("The animal is already added to the animal-type.");
        }

        _animals.Add(animal);

        return Result.Success();
    }

    public Result RemoveAnimal(Animal animal)
    {
        if (!_animals.Any(a => a.Id == animal.Id))
        {
            return Result.NotFound($"The animal-type doesn't have a animal with ID = {animal.Id}.");
        }

        _animals.Remove(animal);

        return Result.Success();
    }

    public Result AddDisease(Disease disease)
    {
        if (_animalTypeDiseases.Any(atd => atd.Disease == disease))
        {
            return Result.Conflict("The disease is already added to the animal type.");
        }

        _animalTypeDiseases.Add(AnimalTypeDisease.Create(disease));

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
