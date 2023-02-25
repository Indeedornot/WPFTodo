using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using WPFTodo.Models;
using WPFTodo.Stores;
using WPFTodo.ViewModels;

namespace WPFTodo.Commands;

public class AddTodoCommand : CommandBase {
    private readonly AddTodoViewModel _viewModel;
    private readonly AppStore _appStore;

    public AddTodoCommand(AddTodoViewModel viewModel, AppStore appStore) {
        _viewModel = viewModel;
        _viewModel.PropertyChanged += (_, _) => OnCanExecutedChanged();

        _appStore = appStore;
    }

    public override bool CanExecute(object? parameter) {
        return _viewModel.HasDescription && _viewModel.HasTitle && base.CanExecute(parameter);
    }

    public override void Execute(object? parameter) {
        Todo newTodo = new(_viewModel.Title, _viewModel.Description);
        _appStore.AddTodo(newTodo);

        _viewModel.Description = string.Empty;
        _viewModel.Title = string.Empty;
        _viewModel.TitleConfirmed = false;
    }
}
