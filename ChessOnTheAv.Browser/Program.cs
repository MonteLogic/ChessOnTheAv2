using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.Themes.Fluent;
using System;
#if (ReactiveUIToolkitChosen)
using Avalonia.ReactiveUI;
#endif
using ChessOnTheAv;

internal sealed partial class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("=== CHESS APP C# DEBUG START ===");
        Console.WriteLine("Starting ChessOnTheAv Browser application...");
        
        try
        {
            Console.WriteLine("Building Avalonia app...");
            var app = BuildAvaloniaApp()
                .WithInterFont()
                .With(new FluentTheme())
#if (ReactiveUIToolkitChosen)
                .UseReactiveUI()
#endif
                .LogToTrace();
            
            Console.WriteLine("Starting browser app...");
            await app.StartBrowserAppAsync("out");
            Console.WriteLine("Browser app started successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in Main: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            throw;
        }
    }

    public static AppBuilder BuildAvaloniaApp()
    {
        Console.WriteLine("Building Avalonia app configuration...");
        return AppBuilder.Configure<App>();
    }
}