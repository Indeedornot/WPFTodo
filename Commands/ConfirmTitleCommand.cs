using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFTodo.Stores;
using WPFTodo.ViewModels;

namespace WPFTodo.Commands;

public class ConfirmTitleCommand : CommandBase {
    private readonly AddTodoViewModel _viewModel;

    public ConfirmTitleCommand(AddTodoViewModel viewModel) {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (_, _) => OnCanExecutedChanged();
    }

    public override bool CanExecute(object? parameter) {
        return _viewModel.HasTitle && base.CanExecute(parameter);
    }

    public override void Execute(object? parameter) {
        _viewModel.TitleConfirmed = true;
    }
}
