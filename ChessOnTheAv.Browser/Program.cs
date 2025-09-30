using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.Themes.Fluent;
#if (ReactiveUIToolkitChosen)
using Avalonia.ReactiveUI;
#endif
using ChessOnTheAv;

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp()
            .WithInterFont()
            .With(new FluentTheme())
#if (ReactiveUIToolkitChosen)
            .UseReactiveUI()
#endif
            .LogToTrace()
            .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>();
}