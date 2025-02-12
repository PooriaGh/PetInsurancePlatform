using Ardalis.Result;
using MediatR;

namespace PetInsurancePlatform.SharedKernel.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>> where TQuery : IQuery<TResponse>
{
}
