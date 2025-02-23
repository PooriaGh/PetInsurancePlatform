using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class City : Entity
{
    public static readonly City None = new();

    // Used by EF Core
    private City() : base()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public Province Province { get; private set; } = Province.None;

    public static Result<City> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(CityErrors.EmptyName);
        }

        return new City
        {
            Name = name,
        };
    }

    public Result Update(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(CityErrors.EmptyName);
        }

        Name = name;

        return Result.Success();
    }
}
