using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using PetInsurancePlatform.Insurance.Application.Dtos;
using PetInsurancePlatform.Insurance.Application.Pets.Commands;

namespace PetInsurancePlatform.Insurance.Endpoints.Pets;

internal sealed class CreatePetEndpoint(ISender sender) : Endpoint<PetRequestDto, Guid>
{
    public override async Task HandleAsync(
        PetRequestDto request,
        CancellationToken cancellationToken)
    {
        var command = new CreatePetCommand(request);

        var result = await sender.Send(command, cancellationToken);

        await SendAsync(
            result.Value,
            StatusCodes.Status200OK,
            cancellationToken);
    }
}
