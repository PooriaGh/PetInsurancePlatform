using Ardalis.Result;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PetInsurancePlatform.Insurance.Application.Data;
using PetInsurancePlatform.Insurance.Domain.Errors;
using PetInsurancePlatform.SharedKernel.Messaging;

namespace PetInsurancePlatform.Insurance.Application.InsurancePlans.Commands;

public sealed class SelectInsurancePlanCommand(Guid insurancePlanId) : ICommand<Guid>
{
    public Guid InsurancePlanId { get; set; } = insurancePlanId;

    internal sealed class Validator : AbstractValidator<SelectInsurancePlanCommand>
    {
        public Validator()
        {
            RuleFor(req => req.InsurancePlanId)
                .NotEmpty();
        }
    }

    internal sealed class Handler(
        IInsuranceDbContext dbContext,
        ILogger<Handler> logger) : ICommandHandler<SelectInsurancePlanCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(SelectInsurancePlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await dbContext.InsurancePlans
                .Include(pt => pt.Policies)
                .FirstOrDefaultAsync(pt => pt.Id == request.InsurancePlanId, cancellationToken);

            if (plan is null)
            {
                return Result.NotFound(InsurancePlanErrors.NotFound(request.InsurancePlanId));
            }

            var policy = plan.CreatePolicy();

            if (!policy.IsSuccess)
            {
                return Result.Error(new ErrorList(policy.Errors));
            }

            try
            {
                await dbContext.SaveChangesAsync(cancellationToken);

                return policy.Value.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while selecting the insurance plan.");

                return Result.Error(PetErrors.NotCreated);
            }
        }
    }
}
