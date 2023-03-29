using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFTodo.Commands;

public class RelayCommand : CommandBase
{
    private readonly Action<object?> execute;
    private readonly Func<object?, bool>? canExecute;

    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public RelayCommand(Action<object?> execute, Func<bool> canExecute)
    {
        this.execute = execute;
        this.canExecute = (_) => canExecute();
    }

    public RelayCommand(Action execute, Func<object?, bool>? canExecute = null)
    {
        this.execute = (_) => execute();
        this.canExecute = canExecute;
    }

    public RelayCommand(Action execute, Func<bool> canExecute)
    {
        this.execute = (_) => execute();
        this.canExecute = (_) => canExecute();
    }

    public override bool CanExecute(object? parameter)
    {
        return (canExecute == null || canExecute(parameter)) && base.CanExecute(parameter);
    }

    public override void Execute(object? parameter)
    {
        execute(parameter);
    }
}

