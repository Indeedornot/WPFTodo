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

    private bool isEdditing = false;
    public bool IsEditting {
        get => isEdditing;
        set {
            if (isEdditing == value) {
                return;
            }

            isEdditing = value;
            OnPropertyChanged(nameof(IsEditting));
        }
    }

    public RelayCommand ToggleCompletedCommand { get; }

    //Open/Close More Menu
    public RelayCommand OpenMoreMenuCommand { get; }
    public RelayCommand CloseMoreMenuCommand { get; }

    //More Menu
    public RelayCommand RemoveTodoCommand { get; }
    public RelayCommand DuplicateTodoCommand { get; }
    public RelayCommand ToggleEditTodoCommand { get; }

    //Do to how WPF handles events for TextChanged we need to use
    ///a middle-man property rather than command
    public string NewTitle {
        get => Todo.Title;
        set {
            Todo.Title = value;

            OnPropertyChanged(nameof(Todo));
            _appStore.ChangeTodo(_todo);
        }
    }
    public string NewDescription {
        get => Todo.Description;
        set {
            Todo.Description = value;

            OnPropertyChanged(nameof(Todo));
            _appStore.ChangeTodo(_todo);
        }
    }

    private readonly AppStore _appStore;
    public TodoDisplayViewModel(Todo todo, AppStore appStore) {
        _todo = todo;
        _appStore = appStore;

        ToggleCompletedCommand = new RelayCommand(ToggleComplete);

        OpenMoreMenuCommand = new RelayCommand(OpenMoreMenu);
        CloseMoreMenuCommand = new RelayCommand(CloseMoreMenu);

        RemoveTodoCommand = new RelayCommand(RemoveTodo);
        DuplicateTodoCommand = new RelayCommand(DuplicateTodo);
        ToggleEditTodoCommand = new RelayCommand(ToggleEditTodo);
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
    private void DuplicateTodo() {
        Todo copy = Todo.Duplicate();
        _appStore.AddTodo(copy);
    }

    private void ToggleEditTodo() {
        IsEditting = !IsEditting;
    }
}
