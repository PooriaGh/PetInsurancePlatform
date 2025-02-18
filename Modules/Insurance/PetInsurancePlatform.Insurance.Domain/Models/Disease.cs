using Ardalis.Result;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.SharedKernel.Abstractions;

namespace PetInsurancePlatform.Insurance.Domain.Models;

public sealed class Disease : Entity
{
    public static readonly Disease None = new();

    // Used by EF Core
    private Disease() : base()
    {
    }

    public string Name { get; private set; } = string.Empty;

    public bool Accepted { get; private set; }

    private readonly List<PetTypeDisease> _petTypeDiseases = [];
    public IReadOnlyCollection<PetTypeDisease> PetTypeDiseases => _petTypeDiseases.AsReadOnly();
    public IReadOnlyCollection<PetType> PetTypes => [.. _petTypeDiseases
        .OrderBy(atd => atd.CreatedAt)
        .Select(d => d.PetType)];

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public static Result<Disease> Create(
        string name,
        bool accepted)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(DiseaseErrors.EmptyName);
        }

        return new Disease
        {
            Name = name,
            Accepted = accepted,
            CreatedAt = DateTime.UtcNow,
        };
    }

    public Result Update(
        string name,
        bool accepted)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Invalid(DiseaseErrors.EmptyName);
        }

        Name = name;
        Accepted = accepted;
        UpdatedAt = DateTime.UtcNow;

        return Result.Success();
    }
}
