using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.Linq;

using WPFTodo.Models;
using WPFTodo.Stores;

namespace WPFTodo.ViewModels;

class HomeViewModel : ViewModelBase
{
    public ObservableCollection<TodoDisplayViewModel> TodoDisplayViewModels { get; set; }
    public AddTodoViewModel AddTodoViewModel { get; }

    public enum SortOptions
    {
        Newest,
        Oldest,
        [Description("Latest Completed")]
        LatestCompleted,
        [Description("Earliest Completed")]
        EarliestCompleted,
        [Description("Title A-Z")]
        Title_AZ,
        [Description("Title Z-A")]
        Title_ZA,
        [Description("Not Completed")]
        NotCompleted,
    }
    private SortOptions _sortBy = SortOptions.Newest;
    public SortOptions SortBy
    {
        get => _sortBy;
        set
        {
            if (_sortBy == value) return;

            _sortBy = value;
            SortTodos();
            OnPropertyChanged(nameof(SortBy));
        }
    }

    private readonly AppStore _appStore;
    public HomeViewModel(AppStore appStore)
    {
        _appStore = appStore;

        var todoDisplayVMs = _appStore.Todos.Select(todo => TodoToTodoDisplayVM(todo));
        TodoDisplayViewModels = new(todoDisplayVMs);
        //SortTodos(); - not needed since we sort by AddedAt at first

        AddTodoViewModel = new(_appStore);

        _appStore.TodoAdded += OnTodoAdded;
        _appStore.TodoRemoved += OnTodoRemoved;
        _appStore.TodoListChanged += OnTodoListChanged;
    }

    private void SortTodos()
    {
        IEnumerable<TodoDisplayViewModel> sortedTodoVMs = SortBy switch
        {
            SortOptions.Newest => TodoDisplayViewModels.OrderByDescending(todoVM => todoVM.Todo.AddedAt),
            SortOptions.Oldest => TodoDisplayViewModels.OrderBy(todoVM => todoVM.Todo.AddedAt),
            SortOptions.LatestCompleted => TodoDisplayViewModels.OrderByDescending(todoVM => todoVM.Todo.IsCompleted).ThenByDescending(todoVM => todoVM.Todo.CompletedAt),
            SortOptions.EarliestCompleted => TodoDisplayViewModels.OrderByDescending(todoVM => todoVM.Todo.IsCompleted).ThenBy(todoVM => todoVM.Todo.CompletedAt),
            SortOptions.NotCompleted => TodoDisplayViewModels.OrderBy(todoVM => todoVM.Todo.IsCompleted),
            SortOptions.Title_AZ => TodoDisplayViewModels.OrderBy(todoVM => todoVM.Todo.Title),
            SortOptions.Title_ZA => TodoDisplayViewModels.OrderByDescending(todoVM => todoVM.Todo.Title),
            _ => TodoDisplayViewModels
        };

        TodoDisplayViewModels = new(sortedTodoVMs);
        OnPropertyChanged(nameof(TodoDisplayViewModels));
    }

    private void OnTodoAdded(Todo todo)
    {
        TodoDisplayViewModels.Insert(0, TodoToTodoDisplayVM(todo));
    }
    private void OnTodoRemoved(Todo todo)
    {
        var viewModel = TodoDisplayViewModels.First(x => x.Todo.Id == todo.Id);
        viewModel.Dispose();
        TodoDisplayViewModels.Remove(viewModel);
    }
    private void OnTodoListChanged()
    {
        var todoDisplayVMs = TodosToTodoDisplayVMs(_appStore.Todos);
        TodoDisplayViewModels = new(todoDisplayVMs);
        SortTodos();

        OnPropertyChanged(nameof(TodoDisplayViewModels));
    }

    private IEnumerable<TodoDisplayViewModel> TodosToTodoDisplayVMs(IEnumerable<Todo> Todos)
    {
        return Todos.Select(todo => TodoToTodoDisplayVM(todo));
    }
    private TodoDisplayViewModel TodoToTodoDisplayVM(Todo todo)
    {
        return new(todo, _appStore);
    }

    public override void Dispose()
    {
        base.Dispose();
        _appStore.TodoAdded -= OnTodoAdded;
        _appStore.TodoRemoved -= OnTodoRemoved;
        _appStore.TodoListChanged -= OnTodoListChanged;
    }
}
