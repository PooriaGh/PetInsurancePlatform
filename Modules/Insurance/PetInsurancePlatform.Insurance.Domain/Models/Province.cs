using Ardalis.Result;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class Province : Entity
{
    public static readonly Province None = new();

    // Used by EF Core
    private Province() : base()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public bool Deleted { get; private set; }

    private readonly List<City> _cities = [];

    public IReadOnlyCollection<City> Cities => _cities.AsReadOnly();

    public static Province Create(string name)
    {
        return new Province
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
        if (_cities.Any(c => c == city))
        {
            return Result.Conflict($"The city is already added to the province.");
        }

        _cities.Add(city);

        return Result.Success();
    }

    public Result RemoveCity(City city)
    {
        if (!_cities.Any(c => c == city))
        {
            return Result.NotFound($"The province doesn't have a city with Id = {city.Id}.");
        }

        _cities.Remove(city);

        return Result.Success();
    }
}
