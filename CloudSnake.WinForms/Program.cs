using Microsoft.Extensions.DependencyInjection;

namespace CloudSnake.WinForms;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        var services = new ServiceCollection();
        services.AddSingleton<MainForm>();
        var serviceProvider = services.BuildServiceProvider();

        ApplicationConfiguration.Initialize();
        Application.Run(serviceProvider.GetService<MainForm>());
    }
}