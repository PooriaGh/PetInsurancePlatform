using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Animals.Domain.Provinces;

public sealed class Province(Guid id) : Entity(id)
{
    public string Name { get; private set; } = null!;

    public bool Deleted { get; private set; }

    private readonly List<City> _cities = [];
    public IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

    public static Province Create(string name)
    {
        return new Province(Guid.NewGuid())
        {
            Name = name,
        };
    }

    public void Update(string name)
    {
        Name = name;
    }

    public Result Remove()
    {
        if (_cities.Count != 0)
        {
            return Result.Conflict($"The province has cities.");
        }

        Deleted = true;

        return Result.Success();
    }

    public Result AddCity(City city)
    {
        if (_cities.Any(c => c.Id == city.Id))
        {
            return Result.Conflict($"The city is already added to the province.");
        }

        _cities.Add(city);

        return Result.Success();
    }

    public Result RemoveCity(City city)
    {
        if (!_cities.Any(c => c.Id == city.Id))
        {
            return Result.NotFound($"The province doesn't have a city with Id = {city.Id}.");
        }

        _cities.Remove(city);

        return Result.Success();
    }
}
