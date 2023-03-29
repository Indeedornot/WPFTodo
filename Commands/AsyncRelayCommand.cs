using System;
using System.Threading.Tasks;

namespace WPFTodo.Commands;
public class AsyncRelayCommand : AsyncCommandBase
{
    private readonly Func<object?, Task> execute;
    private readonly Func<object?, bool>? canExecute;

    public AsyncRelayCommand(Func<object?, Task> execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public override bool CanExecute(object? parameter)
    {
        return canExecute == null || (canExecute is not null && canExecute(parameter));
    }

    public override async Task ExecuteAsync(object? parameter)
    {
        await execute(parameter);
    }
}
