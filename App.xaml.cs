using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using WPFTodo.HostBuilder;
using WPFTodo.Models;
using WPFTodo.Services.DataProvider;
using WPFTodo.Services.NavigationService;

using WPFTodo.Services.Provider;

using WPFTodo.Stores;
using WPFTodo.ViewModels;

namespace WPFTodo;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    private readonly IHost _host;

    public App() {
        _host = Host.CreateDefaultBuilder()
            .AddViewModels()
            .ConfigureServices((context, services) => {
                IPersistentDataManager persistentDataManager = new JsonPersistentDataManager();
                services.AddSingleton(persistentDataManager);

                services.AddSingleton((s) => new AppStore(persistentDataManager));

                services.AddSingleton<NavigationBackService>();
                services.AddSingleton<NavigationStore>();

                services.AddSingleton<MainViewModel>();

                services.AddSingleton(s => new MainWindow() {
                    DataContext = s.GetRequiredService<MainViewModel>()
                });
            })
            .Build();
    }

    protected override void OnStartup(StartupEventArgs e) {
        _host.Start();

        MainWindow = _host.Services.GetRequiredService<MainWindow>();
        MainWindow.Show();

        base.OnStartup(e);
    }

    protected override void OnExit(ExitEventArgs e) {
        SavePersistentData();
        _host.Dispose();
        base.OnExit(e);
    }

    private void SavePersistentData() {
        var appStore = _host.Services.GetRequiredService<AppStore>();
        var persistentDataManager = _host.Services.GetRequiredService<IPersistentDataManager>();
        //...
    }
}
