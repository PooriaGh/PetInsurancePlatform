using Ardalis.Result;
using FastEndpoints;

namespace PetInsurancePlatform.SharedKernel.Messaging;

public interface IQueryHandler<TQuery, TResponse>
    : ICommandHandler<TQuery, Result<TResponse>> where TQuery
    : IQuery<TResponse>
{
}
