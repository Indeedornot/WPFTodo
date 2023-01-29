using System;
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
    public event Action<bool> LoadingChanged;

    private readonly IPersistentDataManager _dataManager;
    public AppStore(IPersistentDataManager dataManager) {
        _dataManager = dataManager;
        _initializeTask = new(Initialize);
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
        //TODO: Load persistend data

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

    #endregion
}
