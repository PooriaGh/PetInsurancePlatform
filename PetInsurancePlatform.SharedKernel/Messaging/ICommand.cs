using Ardalis.Result;
using MediatR;

namespace PetInsurancePlatform.SharedKernel.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
