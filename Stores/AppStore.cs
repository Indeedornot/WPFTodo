using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using WPFTodo.Models;
using WPFTodo.Services.Provider;

namespace WPFTodo.Stores;

public class AppStore {
    private bool _isLoading;
    /// <summary>
    /// Signals fetching of initial data or refetching weatherForecasts
    /// </summary>
    public bool IsLoading {
        get => _isLoading;
        private set {
            if (_isLoading == value) return;

            _isLoading = value;
            LoadingChanged?.Invoke(value);
        }
    }
    public event Action<bool>? LoadingChanged;

    private List<Todo> todos { get; set; } = new();
    /// <summary>
    /// Collection of todos, Sorted by AddedAt
    /// </summary>
    public IEnumerable<Todo> Todos {
        get => todos; private set {
            todos = value.ToList();
            TodoListChanged?.Invoke();
        }
    }

    public event Action? TodoListChanged;

    public event Action<Todo>? TodoAdded;
    public event Action<Todo>? TodoRemoved;
    public event Action<Todo>? TodoChanged;

    private readonly IPersistentDataManager _dataManager;
    public AppStore(IPersistentDataManager dataManager) {
        _dataManager = dataManager;
        _initializeTask = new(Initialize);
    }

    public void AddTodo(Todo todo) {
        todos.Add(todo);
        TodoAdded?.Invoke(todo);
    }

    public void RemoveTodo(Todo todo) {
        todos.Remove(todo);
        TodoRemoved?.Invoke(todo);
    }

    public void ChangeTodo(Todo todo) {
        Todo listTodo = Todos.First(x => x.Id == todo.Id);
        todo.CopyTo(listTodo);
        TodoChanged?.Invoke(todo);
    }

    #region Loading
    /// <summary>
    /// <br/> Initialy loads the data from the persistent storage and fetches weather
    /// </summary>
    /// <returns></returns>
    public async Task Load() {
        try {
            await _initializeTask.Value;
        }
        catch (Exception) {
            _initializeTask = new(Initialize);
            IsLoading = false;
        }
    }

    private Lazy<Task> _initializeTask;
    public bool IsInitialized => _initializeTask.IsValueCreated;

    private async Task Initialize() {
        IsLoading = true;

        PersistentData? persistentData = GetPersistentData();
        if (persistentData != null) {
            todos = persistentData.Todos.OrderByDescending(x => x.AddedAt).ToList();
        }

        IsLoading = false;
    }

    private PersistentData? GetPersistentData() {
        try {
            return _dataManager.GetPersistentData();
        }
        catch {
            return null;
        }
    }

    public PersistentData PersistentData => new() {
        Todos = todos.OrderByDescending(x => x.AddedAt).ToList()
    };

    #endregion
}
