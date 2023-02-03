using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFTodo.Commands;
using WPFTodo.Stores;

namespace WPFTodo.ViewModels;
public class AddTodoViewModel : ViewModelBase {
    private string title;
    public string Title {
        get => title;
        set {
            if (value == title) return;

            title = value;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(HasTitle));
        }
    }
    public bool HasTitle => !string.IsNullOrWhiteSpace(Title);

    private bool _titleConfirmed;
    public bool TitleConfirmed {
        get => _titleConfirmed;
        set {
            if (_titleConfirmed == value) return;

            _titleConfirmed = value;
            OnPropertyChanged(nameof(TitleConfirmed));
        }
    }

    private string description;
    public string Description {
        get => description;
        set {
            if (value == description) return;

            description = value;
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(HasDescription));
        }
    }
    public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    public AddTodoCommand AddTodoCommand { get; }
    public EditTittleCommand EditTittleCommand { get; }
    public ConfirmTitleCommand ConfirmTitleCommand { get; }

    public AddTodoViewModel(AppStore appStore) {
        AddTodoCommand = new AddTodoCommand(this, appStore);
        ConfirmTitleCommand = new ConfirmTitleCommand(this);
        EditTittleCommand = new EditTittleCommand(this);
    }
}

