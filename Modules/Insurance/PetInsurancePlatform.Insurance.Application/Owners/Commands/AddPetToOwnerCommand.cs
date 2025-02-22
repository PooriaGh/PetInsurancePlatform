using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.SharedKernel.Messaging;

namespace PetInsurancePlatform.Insurance.Application.Owners.Commands;

public sealed class AddPetToOwnerCommand(
    Guid ownerId,
    Guid petId) : ICommandWithResult<Guid>
{
    public Guid OwnerId { get; set; } = ownerId;

    public Guid PetId { get; set; } = petId;

    internal sealed class RequestValidator : Validator<AddPetToOwnerCommand>
    {
        public RequestValidator()
        {
            RuleFor(req => req.OwnerId)
                .NotEmpty();

            RuleFor(req => req.PetId)
                .NotEmpty();
        }
    }

    internal sealed class Handler(
        IInsuranceDbContext dbContext,
        ILogger<Handler> logger) : ICommandWithResultHandler<AddPetToOwnerCommand, Guid>
    {
        public async Task<Result<Guid>> ExecuteAsync(AddPetToOwnerCommand request, CancellationToken cancellationToken)
        {
            var owner = await dbContext.Owners
                .Include(o => o.Pets)
                .FirstOrDefaultAsync(o => o.Id == request.OwnerId, cancellationToken);

            if (owner is null)
            {
                return Result.NotFound(OwnerErrors.NotFound(request.OwnerId));
            }

            var pet = await dbContext.Pets
                .FirstOrDefaultAsync(p => p.Id == request.PetId, cancellationToken);

            if (pet is null)
            {
                return Result.NotFound(PetTypeErrors.NotFound(request.PetId));
            }

            var result = owner.AddPet(pet);

            dbContext.Pets.Add(result.Value);

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);

                return result.Value.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding pet to the owner.");

                return Result.Error(PetErrors.NotCreated);
            }
        }
    }
}
