using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFTodo.ViewModels;

namespace WPFTodo.Commands;
public class EditTittleCommand : CommandBase {
    private readonly AddTodoViewModel _viewModel;

    public EditTittleCommand(AddTodoViewModel viewModel) {
        _viewModel = viewModel;
    }

    public override void Execute(object? parameter) {
        _viewModel.TitleConfirmed = false;
    }
}
