using CloudSnake.Client;
using Microsoft.Extensions.DependencyInjection;

namespace CloudSnake.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        var services = new ServiceCollection();
        services.AddSingleton<GameClient>();
        services.AddSingleton<MainForm>();
        var serviceProvider = services.BuildServiceProvider();

        ApplicationConfiguration.Initialize();
        Application.Run(serviceProvider.GetService<MainForm>());
    }
}