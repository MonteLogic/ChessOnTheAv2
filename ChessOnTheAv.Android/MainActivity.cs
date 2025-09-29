using Android.App;
using Android.Content.PM;
using Avalonia;
using Avalonia.Android;
#if (ReactiveUIToolkitChosen)
using Avalonia.ReactiveUI;
#endif
using ChessOnTheAv;

namespace ChessOnTheAv.Android;

[Activity(
    Label = "ChessOnTheAv.Android",
    Theme = "@style/MyTheme.NoActionBar",
    Icon = "@drawable/icon",
    MainLauncher = true,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.UiMode)]
public class MainActivity : AvaloniaMainActivity<App>
{
    protected override AppBuilder CustomizeAppBuilder(AppBuilder builder)
    {
        return base.CustomizeAppBuilder(builder)
#if (CommunityToolkitChosen)
            .WithInterFont();
#else
            .WithInterFont()
            .UseReactiveUI();
#endif
    }
}
