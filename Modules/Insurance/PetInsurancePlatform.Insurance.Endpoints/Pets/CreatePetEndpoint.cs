using FastEndpoints;
using PetInsurancePlatform.Insurance.Application.Dtos;
using PetInsurancePlatform.Insurance.Application.Pets.Commands;
using PetInsurancePlatform.SharedKernel.Extensions;

namespace PetInsurancePlatform.Insurance.Endpoints.Pets;

internal sealed class CreatePetEndpoint : Endpoint<PetRequestDto, Guid>
{
    public override void Configure()
    {
        Post("pets");
        AllowAnonymous();
    }

    public override async Task HandleAsync(
        PetRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePetCommand(request);

        var result = await command.ExecuteAsync(cancellationToken);

        await this.SendResponse(result, res => res.Value);
    }
}
