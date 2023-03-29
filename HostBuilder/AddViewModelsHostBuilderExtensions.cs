using System;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting;
using WPFTodo.Services.NavigationService;
using WPFTodo.ViewModels;

namespace WPFTodo.HostBuilder;

public static class AddViewModelsHostBuilderExtensions
{
    public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
    {
        hostBuilder.ConfigureServices(services =>
        {
            services.AddTransient<HomeViewModel>();
            services.AddSingleton<Func<HomeViewModel>>((s) => () => s.GetRequiredService<HomeViewModel>());
            services.AddSingleton<NavigationService<HomeViewModel>>();

            services.AddSingleton<MainViewModel>();
        });

        return hostBuilder;
    }
}
