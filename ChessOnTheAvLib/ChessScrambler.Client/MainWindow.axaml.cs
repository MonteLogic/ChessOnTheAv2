using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ChessScrambler.Client.ViewModels;
using ChessScrambler.Client.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace ChessScrambler.Client;

public partial class MainWindow : Window
{
    private ChessBoardViewModel? _viewModel;
    private bool _isLoadingNewPosition = false;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new ChessBoardViewModel();
        _viewModel = DataContext as ChessBoardViewModel;
        
        // Add UI event logging
        SetupUILogging();
        
        // Subscribe to settings changes for window resizing
        if (_viewModel?.AppSettings != null)
        {
            _viewModel.AppSettings.PropertyChanged += OnAppSettingsChanged;
            
            // Log initial window size and settings
            Console.WriteLine($"[WINDOW] Initial window size: {this.Width}x{this.Height}");
            Console.WriteLine($"[SETTINGS] Loaded settings - Board: {_viewModel.AppSettings.BoardSize}px, Window: {_viewModel.AppSettings.WindowWidth}x{_viewModel.AppSettings.WindowHeight}, Mode: {_viewModel.AppSettings.WindowSizeMode}");
            
            // Apply the loaded window size immediately
            this.Width = _viewModel.AppSettings.WindowWidth;
            this.Height = _viewModel.AppSettings.WindowHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Console.WriteLine($"[WINDOW] Applied loaded size: {this.Width}x{this.Height}");
        }
    }

    private void SetupUILogging()
    {
        // Log window events
        this.Opened += (s, e) => LogUIEvent("Window", "Opened", "Application started");
        this.Closing += (s, e) => LogUIEvent("Window", "Closing", "Application closing");
        
        // Log keyboard events
        this.KeyDown += (s, e) => LogUIEvent("Keyboard", "KeyDown", $"Key: {e.Key}, Modifiers: {e.KeyModifiers}");
        this.KeyUp += (s, e) => LogUIEvent("Keyboard", "KeyUp", $"Key: {e.Key}");
        
        // Log mouse events
        this.PointerMoved += (s, e) => {
            var position = e.GetPosition(this);
            if (position.X % 50 == 0 || position.Y % 50 == 0) // Log every 50 pixels to avoid spam
                LogUIEvent("Mouse", "Moved", $"Position: ({position.X:F0}, {position.Y:F0})");
        };
        
        this.PointerEntered += (s, e) => LogUIEvent("Mouse", "Entered", "Mouse entered window");
        this.PointerExited += (s, e) => LogUIEvent("Mouse", "Exited", "Mouse left window");
        
        // Log focus events
        this.GotFocus += (s, e) => LogUIEvent("Focus", "GotFocus", "Window gained focus");
        this.LostFocus += (s, e) => LogUIEvent("Focus", "LostFocus", "Window lost focus");
        
        // Log window resize events (simplified)
        this.Resized += (s, e) => 
        {
            LogUIEvent("Window", "Resized", $"Window resized to {this.Width}x{this.Height}");
            Console.WriteLine($"[WINDOW] Current window size: {this.Width}x{this.Height}");
        };
        
        // Log window position changes (simplified)
        this.PositionChanged += (s, e) => LogUIEvent("Window", "PositionChanged", "Window position changed");
    }

    private void LogUIEvent(string category, string action, string details)
    {
        if (Program.EnableUILogging)
        {
            Console.WriteLine($"[UI] {category}.{action}: {details}");
        }
    }

    private void OnAppSettingsChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is AppSettings settings)
        {
            Console.WriteLine($"[SETTINGS] Property changed: {e.PropertyName}");
            
            if (e.PropertyName == nameof(AppSettings.WindowWidth) || e.PropertyName == nameof(AppSettings.WindowHeight))
            {
                Console.WriteLine($"[SETTINGS] Window size changed - New: {settings.WindowWidth}x{settings.WindowHeight}, Current: {this.Width}x{this.Height}");
                
                // Resize the window when settings change
                this.Width = settings.WindowWidth;
                this.Height = settings.WindowHeight;
                
                // Center the window after resizing
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                
                LogUIEvent("Window", "Resized", $"Window resized to {settings.WindowWidth}x{settings.WindowHeight}");
                Console.WriteLine($"[WINDOW] Applied new size: {this.Width}x{this.Height}");
            }
            else if (e.PropertyName == nameof(AppSettings.BoardSize))
            {
                Console.WriteLine($"[SETTINGS] Board size changed to: {settings.BoardSize}px");
            }
            else if (e.PropertyName == nameof(AppSettings.WindowSizeMode))
            {
                Console.WriteLine($"[SETTINGS] Window size mode changed to: {settings.WindowSizeMode}");
            }
        }
    }

    private void Square_PointerPressed(object sender, PointerPressedEventArgs e)
    {
        LogUIEvent("ChessBoard", "SquareClicked", $"Sender: {sender?.GetType().Name}");
        
        if (sender is Border border && border.DataContext is SquareViewModel square)
        {
            LogUIEvent("ChessBoard", "SquareClicked", $"Square: Row={square.Row}, Col={square.Column}, Piece={square.Piece?.GetType().Name ?? "Empty"}");
            _viewModel?.OnSquareClicked(square);
        }
        else
        {
            LogUIEvent("ChessBoard", "SquareClicked", "Invalid sender or data context");
        }
    }

    private async void NewPosition_Click(object sender, RoutedEventArgs e)
    {
        // Prevent multiple rapid clicks
        if (_isLoadingNewPosition)
        {
            LogUIEvent("Button", "NewPosition", "Button click ignored - already loading");
            Console.WriteLine("[DEBUG] NewPosition_Click ignored - already loading");
            return;
        }

        LogUIEvent("Button", "NewPosition", "New position button clicked");
        Console.WriteLine("[DEBUG] NewPosition_Click called");
        Console.WriteLine($"[DEBUG] ViewModel is null: {_viewModel == null}");
        
        if (_viewModel != null)
        {
            _isLoadingNewPosition = true;
            try
            {
                Console.WriteLine("[DEBUG] Calling LoadNewPosition on ViewModel");
                await _viewModel.LoadNewPosition();
                Console.WriteLine("[DEBUG] LoadNewPosition call completed");
            }
            finally
            {
                _isLoadingNewPosition = false;
            }
        }
        else
        {
            Console.WriteLine("[DEBUG] ERROR: ViewModel is null, cannot load new position");
        }
    }

    private void ResetBoard_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "ResetBoard", "Reset board button clicked");
        _viewModel?.LoadMiddlegamePosition();
    }

    private void Hint_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "Hint", "Hint button clicked");
        // TODO: Implement hint system
        // This could show the best move or highlight tactical opportunities
    }

    private void ExportDebug_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "ExportDebug", "Export debug button clicked");
        _viewModel?.ExportDebugState();
    }

    // Text selection logging for Move History

    private void MoveHistory_GotFocus(object sender, GotFocusEventArgs e)
    {
        LogUIEvent("TextSelection", "MoveHistory", "Move history text box gained focus");
    }

    private void MoveHistory_LostFocus(object sender, RoutedEventArgs e)
    {
        LogUIEvent("TextSelection", "MoveHistory", "Move history text box lost focus");
    }

    private void MoveHistory_KeyDown(object sender, KeyEventArgs e)
    {
        LogUIEvent("TextSelection", "MoveHistory", $"Key pressed: {e.Key}, Modifiers: {e.KeyModifiers}");
        
        // Log copy operations
        if (e.Key == Key.C && e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            LogUIEvent("TextSelection", "MoveHistory", "Copy operation (Ctrl+C) detected");
        }
    }

    // Text selection logging for Instructions

    private void Instructions_GotFocus(object sender, GotFocusEventArgs e)
    {
        LogUIEvent("TextSelection", "Instructions", "Instructions text box gained focus");
    }

    private void Instructions_LostFocus(object sender, RoutedEventArgs e)
    {
        LogUIEvent("TextSelection", "Instructions", "Instructions text box lost focus");
    }

    private void Instructions_KeyDown(object sender, KeyEventArgs e)
    {
        LogUIEvent("TextSelection", "Instructions", $"Key pressed: {e.Key}, Modifiers: {e.KeyModifiers}");
        
        // Log copy operations
        if (e.Key == Key.C && e.KeyModifiers.HasFlag(KeyModifiers.Control))
        {
            LogUIEvent("TextSelection", "Instructions", "Copy operation (Ctrl+C) detected");
        }
    }

    private void NewGame_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "NewGame", "New game button clicked from popup");
        _viewModel?.LoadNewPosition();
        _viewModel?.CloseGameEndPopup();
    }

    private void ClosePopup_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "ClosePopup", "Close popup button clicked");
        _viewModel?.CloseGameEndPopup();
    }

    private void GoToFirstMove_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "GoToFirstMove", "Go to first move button clicked");
        _viewModel?.GoToFirstMove();
    }

    private void GoToPreviousMove_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "GoToPreviousMove", "Go to previous move button clicked");
        _viewModel?.GoToPreviousMove();
    }

    private void GoToNextMove_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "GoToNextMove", "Go to next move button clicked");
        _viewModel?.GoToNextMove();
    }

    private void GoToLastMove_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "GoToLastMove", "Go to last move button clicked");
        _viewModel?.GoToLastMove();
    }

    private async void ImportPgnFile_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "ImportPgnFile", "Import PGN file button clicked");
        
        try
        {
            var options = new FilePickerOpenOptions
            {
                Title = "Select PGN File",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("PGN Files")
                    {
                        Patterns = new[] { "*.pgn" }
                    },
                    new FilePickerFileType("All Files")
                    {
                        Patterns = new[] { "*.*" }
                    }
                }
            };

            var files = await StorageProvider.OpenFilePickerAsync(options);
            
            if (files.Count > 0 && files[0] is IStorageFile file)
            {
                var filePath = file.Path.LocalPath;
                _viewModel?.ImportGamesFromFile(filePath);
            }
        }
        catch (Exception ex)
        {
            LogUIEvent("Error", "ImportPgnFile", $"Error importing PGN file: {ex.Message}");
        }
    }

    private void ClearGamesBank_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "ClearGamesBank", "Clear games bank button clicked");
        _viewModel?.ClearImportedGames();
    }

    private void ExportCurrentGamePgn_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "ExportCurrentGamePgn", "Export current game PGN button clicked");
        _viewModel?.ExportCurrentGamePgn();
    }

    private void SaveSettings_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "SaveSettings", "Save settings button clicked");
        _viewModel?.AppSettings?.SaveSettings();
        
        // Log current settings
        if (_viewModel?.AppSettings != null)
        {
            Console.WriteLine($"[SETTINGS] Manual save - Board: {_viewModel.AppSettings.BoardSize}px, Window: {_viewModel.AppSettings.WindowWidth}x{_viewModel.AppSettings.WindowHeight}, Mode: {_viewModel.AppSettings.WindowSizeMode}");
        }
    }

    private void ResetSettings_Click(object sender, RoutedEventArgs e)
    {
        LogUIEvent("Button", "ResetSettings", "Reset settings button clicked");
        _viewModel?.AppSettings?.ResetToDefaults();
        
        // Update the window size immediately
        if (_viewModel?.AppSettings != null)
        {
            this.Width = _viewModel.AppSettings.WindowWidth;
            this.Height = _viewModel.AppSettings.WindowHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Console.WriteLine($"[SETTINGS] Reset - Board: {_viewModel.AppSettings.BoardSize}px, Window: {_viewModel.AppSettings.WindowWidth}x{_viewModel.AppSettings.WindowHeight}, Mode: {_viewModel.AppSettings.WindowSizeMode}");
        }
    }

    protected override void OnClosed(EventArgs e)
    {
        // Unsubscribe from settings changes
        if (_viewModel?.AppSettings != null)
        {
            _viewModel.AppSettings.PropertyChanged -= OnAppSettingsChanged;
        }
        
        base.OnClosed(e);
    }

}