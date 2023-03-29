using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

using WPFTodo.Commands;
using WPFTodo.Models;
using WPFTodo.Stores;

namespace WPFTodo.ViewModels;
public class AddTodoViewModel : ViewModelBase
{
    private string title = string.Empty;
    public string Title
    {
        get => title;
        set
        {
            if (value == title) return;

            title = value;
            OnPropertyChanged(nameof(Title));
            OnPropertyChanged(nameof(HasTitle));
        }
    }
    public bool HasTitle => !string.IsNullOrWhiteSpace(Title);

    private bool _titleConfirmed;
    public bool TitleConfirmed
    {
        get => _titleConfirmed;
        set
        {
            if (_titleConfirmed == value) return;

            _titleConfirmed = value;
            OnPropertyChanged(nameof(TitleConfirmed));
        }
    }

    private string description = string.Empty;
    public string Description
    {
        get => description;
        set
        {
            if (value == description) return;

            description = value;
            OnPropertyChanged(nameof(Description));
            OnPropertyChanged(nameof(HasDescription));
        }
    }
    public bool HasDescription => !string.IsNullOrWhiteSpace(Description);

    public RelayCommand AddTodoCommand { get; }
    public RelayCommand EditTittleCommand { get; }
    public RelayCommand ConfirmTitleCommand { get; }

    private readonly AppStore _appStore;
    public AddTodoViewModel(AppStore appStore)
    {
        _appStore = appStore;

        AddTodoCommand = new RelayCommand(AddTodo, CanAddTodo);
        this.PropertyChanged += (_, _) => AddTodoCommand.OnCanExecutedChanged();

        ConfirmTitleCommand = new RelayCommand(ConfirmTitle, CanConfirmTitle);
        this.PropertyChanged += (_, _) => ConfirmTitleCommand.OnCanExecutedChanged();

        EditTittleCommand = new RelayCommand(EditTitle);
    }

    private void AddTodo()
    {
        Todo newTodo = new(Title, Description);
        _appStore.AddTodo(newTodo);

        Description = string.Empty;
        Title = string.Empty;
        TitleConfirmed = false;
    }
    private bool CanAddTodo()
    {
        return HasDescription && HasTitle;
    }

    private void ConfirmTitle()
    {
        TitleConfirmed = true;
    }
    private bool CanConfirmTitle()
    {
        return HasTitle;
    }

    private void EditTitle()
    {
        TitleConfirmed = false;
    }
}

