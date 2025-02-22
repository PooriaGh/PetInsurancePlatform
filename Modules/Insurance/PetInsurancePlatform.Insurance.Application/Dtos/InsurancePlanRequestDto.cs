namespace PetInsurancePlatform.Insurance.Application.Dtos;

public sealed class InsurancePlanRequestDto
{
    public string Name { get; set; } = string.Empty;

    public bool VIP { get; set; }

    public long Price { get; set; }

    public List<InsuranceCoverageRequestDto> Coverages { get; set; } = [];
}
