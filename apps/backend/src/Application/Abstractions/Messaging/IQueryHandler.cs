using MediatR;
using SharedKernel.Results;

namespace Application.Abstractions.Messaging;

internal interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>;
