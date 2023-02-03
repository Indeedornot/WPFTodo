using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using WPFTodo.Models;
using WPFTodo.Stores;

namespace WPFTodo.ViewModels;

class HomeViewModel : ViewModelBase {
    public ObservableCollection<TodoDisplayViewModel> TodoDisplayViewModels { get; }

    private readonly AppStore _appStore;
    public HomeViewModel(AppStore appStore) {
        _appStore = appStore;

        var todoDisplayVMs = _appStore.Todos.Select(todo => NewTodoDisplayVM(todo));
        TodoDisplayViewModels = new(todoDisplayVMs);


        _appStore.TodoAdded += OnTodoAdded;
        _appStore.TodoRemoved += OnTodoRemoved;
        _appStore.TodoListChanged += OnTodoListChanged;
    }

    private void OnTodoAdded(Todo todo) {
        TodoDisplayViewModels.Insert(0, NewTodoDisplayVM(todo));
    }

    private void OnTodoRemoved(Todo todo) {
        var viewModel = TodoDisplayViewModels.First(x => x.Todo.Id == todo.Id);
        TodoDisplayViewModels.Remove(viewModel);
    }

    private void OnTodoListChanged() {
        TodoDisplayViewModels.Clear();
        IEnumerable<Todo> todos = _appStore.Todos;

        foreach (Todo todo in todos) {
            TodoDisplayViewModels.Add(NewTodoDisplayVM(todo));
        }
    }

    private TodoDisplayViewModel NewTodoDisplayVM(Todo todo) {
        return new TodoDisplayViewModel(todo, _appStore);
    }

    public override void Dispose() {
        base.Dispose();
        _appStore.TodoAdded -= OnTodoAdded;
        _appStore.TodoRemoved -= OnTodoRemoved;
        _appStore.TodoListChanged -= OnTodoListChanged;
    }
}
