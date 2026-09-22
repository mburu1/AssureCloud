using System;
using System.Threading;
using System.Threading.Tasks;

namespace AssureCloud.Application.Behaviors;

using AssureCloud.Application.Abstractions;

public class ValidationBehavior<TCommand> : ICommandHandler<TCommand> where TCommand : ICommand
{
    private readonly ICommandHandler<TCommand> _inner;

    public ValidationBehavior(ICommandHandler<TCommand> inner)
    {
        _inner = inner;
    }

    public async Task HandleAsync(TCommand command, CancellationToken ct = default)
    {
        await _inner.HandleAsync(command, ct);
    }
}
