using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Errors;
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

    public static Result<Province> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(ProvinceErrors.EmptyName);
        }

        return new Province
        {
            Name = name,
        };
    }

    public Result Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(ProvinceErrors.EmptyName);
        }

        Name = name;

        return Result.Success();
    }

    public Result Remove()
    {
        if (_cities.Count != 0)
        {
            return Result.Invalid(ProvinceErrors.NotRemovable);
        }

        Deleted = true;

        return Result.Success();
    }

    public Result AddCity(City city)
    {
        if (_cities.Any(c => c == city))
        {
            return Result.Conflict(ProvinceErrors.DuplicateCity(city.Name));
        }

        _cities.Add(city);

        return Result.Success();
    }

    public Result RemoveCity(City city)
    {
        if (!_cities.Any(c => c == city))
        {
            return Result.NotFound(ProvinceErrors.NotFoundCity(city.Id));
        }

        _cities.Remove(city);

        return Result.Success();
    }
}
