using Ardalis.Result;
using FastEndpoints;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Application.Dtos;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.Insurance.Domain.Models;
using PetInsurancePlatform.SharedKernel.Messaging;

namespace PetInsurancePlatform.Insurance.Application.Owners.Commands;

public sealed class CreateOwnerCommand(OwnerRequestDto ownerRequest) : ICommandWithResult
{
    public OwnerRequestDto OwnerRequest { get; set; } = ownerRequest;

    internal sealed class RequestValidator : Validator<CreateOwnerCommand>
    {
        public RequestValidator()
        {
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
        }
    }

    public class AddOwnerCommandHandler(
        IInsuranceDbContext dbContext,
        ILogger<AddOwnerCommandHandler> logger) : ICommandWithResultHandler<CreateOwnerCommand>
    {
        public async Task<Result> ExecuteAsync(CreateOwnerCommand command, CancellationToken ct)
        {
            var owner = Owner.Create(
                command.OwnerRequest.FirstName,
                command.OwnerRequest.LastName,
                command.OwnerRequest.NationalID,
                command.OwnerRequest.DateOfBirth);

            if (!owner.IsSuccess)
            {
                return Result.Error(new ErrorList(owner.Errors));
            }

            dbContext.Owners.Add(owner.Value);

            try
            {
                await dbContext.SaveChangesAsync(ct);

                return Result.Success();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while creating owner.");

                return Result.Error(OwnerErrors.NotCreated);
            }
        }
    }
}
