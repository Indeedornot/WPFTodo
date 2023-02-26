using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WPFTodo.Commands;
using WPFTodo.Models;
using WPFTodo.Stores;

namespace WPFTodo.ViewModels;
public class TodoDisplayViewModel : ViewModelBase {
    private Todo _todo;
    public Todo Todo {
        get => _todo;
        set {
            if (_todo == value) {
                return;
            }

            _todo = value;
            OnPropertyChanged(nameof(Todo));
        }
    }

    private bool displayMoreMenu = false;
    public bool DisplayMoreMenu {
        get => displayMoreMenu;
        set {
            if (displayMoreMenu == value) {
                return;
            }

            displayMoreMenu = value;
            OnPropertyChanged(nameof(DisplayMoreMenu));
        }
    }

    public RelayCommand ToggleCompletedCommand { get; }
    public RelayCommand OpenMoreMenuCommand { get; set; }
    public RelayCommand CloseMoreMenuCommand { get; set; }
    public RelayCommand RemoveTodoCommand { get; set; }

    private readonly AppStore _appStore;
    public TodoDisplayViewModel(Todo todo, AppStore appStore) {
        _todo = todo;
        _appStore = appStore;

        ToggleCompletedCommand = new RelayCommand(ToggleComplete);
        OpenMoreMenuCommand = new RelayCommand(OpenMoreMenu);
        CloseMoreMenuCommand = new RelayCommand(CloseMoreMenu);
        RemoveTodoCommand = new RelayCommand(RemoveTodo);
    }

    private void ToggleComplete() {
        _todo.CompletedAt = _todo.CompletedAt == null ? DateTime.Now : null;
        _appStore.ChangeTodo(_todo);
        OnPropertyChanged(nameof(Todo));
    }

    private void OpenMoreMenu() {
        DisplayMoreMenu = true;
    }
    private void CloseMoreMenu() {
        DisplayMoreMenu = false;
    }

    private void RemoveTodo() {
        _appStore.RemoveTodo(_todo);
    }
}
