using FastEndpoints;
using PetInsurancePlatform.Insurance.Application.InsurancePlans.Commands;
using PetInsurancePlatform.SharedKernel.Extensions;

namespace PetInsurancePlatform.Insurance.Endpoints.InsurancePlans.AddPet;

public sealed class AddPetEndpoint : Endpoint<AddPetRequest, Guid>
{
    public override void Configure()
    {
        Post("plans/policies/pets");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        AddPetRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddPetToInsurancePolicyCommand(
            request.InsurancePlanId,
            request.InsurancePolicyId,
            request.PetRequest);

        var result = await command.ExecuteAsync(cancellationToken);

        await this.SendResponse(result, res => res.Value);
    }
}
