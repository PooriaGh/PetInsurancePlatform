using PetInsurancePlatform.Insurance.Domain.Enums;

namespace PetInsurancePlatform.Insurance.Application.Dtos;

public sealed class PetRequestDto
{
    public static readonly PetRequestDto None = new();

    public string Name { get; set; } = string.Empty;

    public string Breed { get; set; } = string.Empty;

    public Gender Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public Guid PetTypeId { get; set; }

    public long Price { get; set; }

    public Guid CityId { get; set; }

    public AddressDto Address { get; set; } = AddressDto.None;

    public List<string> Appearances { get; set; } = [];

    public string MicrochipCode { get; set; } = string.Empty;

    public List<Guid> DiseasesIds { get; set; } = [];
}
