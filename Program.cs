using Microsoft.Extensions.DependencyInjection;
using VirtualDesktopWidget.Services;
using VirtualDesktopWidget.Views;

namespace VirtualDesktopWidget;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var serviceProvider = new ServiceCollection()
            .AddSingleton<HookService>()
            .AddSingleton<InputService>()
            .AddSingleton<VirtualDesktopService>()
            .AddSingleton<MainAppContext>()
            .BuildServiceProvider();

        Application.Run(serviceProvider.GetRequiredService<MainAppContext>());
    }
}
