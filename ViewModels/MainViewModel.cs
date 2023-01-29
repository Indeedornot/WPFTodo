using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using WPFTodo.Services.NavigationService;
using WPFTodo.Stores;

namespace WPFTodo.ViewModels;
class MainViewModel : ViewModelBase {
    private readonly NavigationStore _navigationStore;
    private readonly AppStore _appStore;

    private readonly NavigationService<HomeViewModel> _navigationHome;

    public ViewModelBase? CurrentViewModel => _navigationStore.CurrentViewModel;

    public MainViewModel(NavigationStore navigationStore, AppStore appStore, NavigationService<HomeViewModel> navigationHome) {
        _navigationHome = navigationHome;
        _appStore = appStore;

        _navigationStore = navigationStore;
        _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;

        SetInitialView();
    }

    private async Task SetInitialView() {
        await _appStore.Load();
        _navigationHome.Navigate();
    }

    private void OnCurrentViewModelChanged() {
        OnPropertyChanged(nameof(CurrentViewModel));
    }
}
