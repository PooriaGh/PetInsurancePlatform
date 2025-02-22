namespace PetInsurancePlatform.Insurance.Application.Dtos;

public sealed class OwnerRequestDto
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public long NationalID { get; set; }

    public DateOnly DateOfBirth { get; set; }
}
