using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Application.Dtos;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.Insurance.Domain.Models;

namespace PetInsurancePlatform.Insurance.Application.Pets.Commands;

public sealed class AddOwnerCommand(
    Guid petId,
    OwnerRequestDto ownerRequest) : ICommand<Result>
{
    public Guid PetId { get; set; } = petId;

    public OwnerRequestDto OwnerRequest { get; set; } = ownerRequest;

    internal sealed class RequestValidator : Validator<AddOwnerCommand>
    {
        public RequestValidator()
        {
            RuleFor(req => req.PetId)
                .NotEmpty();

            RuleFor(req => req.OwnerRequest)
                .NotEmpty();

            RuleFor(req => req.OwnerRequest.FirstName)
                .NotEmpty();

            RuleFor(req => req.OwnerRequest.LastName)
                .NotEmpty();

            RuleFor(req => req.OwnerRequest.NationalID)
                .IsInEnum();

            RuleFor(req => req.OwnerRequest.DateOfBirth)
                .NotEmpty();

            RuleFor(req => req.OwnerRequest.CityId)
                .NotEmpty();
        }
    }

    public class AddOwnerCommandHandler(
        IInsuranceDbContext dbContext,
        ILogger<AddOwnerCommandHandler> logger) : ICommandHandler<AddOwnerCommand, Result>
    {
        public async Task<Result> ExecuteAsync(AddOwnerCommand command, CancellationToken ct)
        {
            var pet = await dbContext.Pets
                .FirstOrDefaultAsync(pt => pt.Id == command.PetId, ct);

            if (pet is null)
            {
                return Result.NotFound(PetErrors.NotFound(command.PetId));
            }

            var owner = await CreateOwnerAsync(command.OwnerRequest, ct);

            if (!owner.IsSuccess)
            {
                return Result.Error(new ErrorList(owner.Errors));
            }

            var result = pet.AddOwner(owner);

            if (!result.IsSuccess)
            {
                return Result.Error(new ErrorList(result.Errors));
            }

            try
            {
                await dbContext.SaveChangesAsync(ct);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while adding owner to the pet.");

                return Result.Error(OwnerErrors.NotCreated);
            }
        }

        private async Task<Result<Owner>> CreateOwnerAsync(
            OwnerRequestDto ownerRequest,
            CancellationToken cancellationToken)
        {
            var city = await dbContext.Cities
                .FirstOrDefaultAsync(pt => pt.Id == ownerRequest.CityId, cancellationToken);

            if (city is null)
            {
                return Result.NotFound(CityErrors.NotFound(ownerRequest.CityId));
            }

            return Owner.Create(
                ownerRequest.FirstName,
                ownerRequest.LastName,
                ownerRequest.NationalID,
                ownerRequest.DateOfBirth,
                city);
        }
    }
}
