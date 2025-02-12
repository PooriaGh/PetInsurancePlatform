using Ardalis.Result;
using MediatR;

namespace PetInsurancePlatform.SharedKernel.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
