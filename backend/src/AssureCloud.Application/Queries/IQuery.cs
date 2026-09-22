using System.Threading;
using System.Threading.Tasks;
using AssureCloud.Application.Abstractions;

namespace AssureCloud.Application.Queries;

public interface IQuery
{
}

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken ct = default);
}
