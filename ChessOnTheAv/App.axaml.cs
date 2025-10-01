using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System;
#if (CommunityToolkitChosen)
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using System.Linq;
#endif
using Avalonia.Markup.Xaml;
using ChessOnTheAv.ViewModels;
using ChessOnTheAv.Views;

namespace ChessOnTheAv;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Console.WriteLine("=== APP INITIALIZATION DEBUG ===");
        Console.WriteLine($"ApplicationLifetime type: {ApplicationLifetime?.GetType().Name}");
        
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            Console.WriteLine("Using desktop application lifetime");
#if (CommunityToolkitChosen)
            // Avoid duplicate validations from both Avalonia and the CommunityToolkit. 
            // More info: https://docs.avaloniaui.net/docs/guides/development-guides/data-validation#manage-validationplugins
            DisableAvaloniaDataAnnotationValidation();
#endif
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainViewModel()
            };
            Console.WriteLine("Desktop MainWindow created");
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            Console.WriteLine("Using single view application lifetime (WASM)");
            // Use MainView directly for WASM
            try
            {
                var mainView = new MainView();
                var viewModel = new MainViewModel();
                mainView.DataContext = viewModel;
                singleViewPlatform.MainView = mainView;
                Console.WriteLine("Single view MainView created successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR creating MainView: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }
        else
        {
            Console.WriteLine($"WARNING: Unknown application lifetime type: {ApplicationLifetime?.GetType().Name}");
        }

        base.OnFrameworkInitializationCompleted();
        Console.WriteLine("App initialization completed");
    }

#if (CommunityToolkitChosen)
    private void DisableAvaloniaDataAnnotationValidation()
    {
        // Get an array of plugins to remove
        var dataValidationPluginsToRemove =
            BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

        // remove each entry found
        foreach (var plugin in dataValidationPluginsToRemove)
        {
            BindingPlugins.DataValidators.Remove(plugin);
        }
    }
#endif
}