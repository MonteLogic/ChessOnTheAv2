using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using ChessOnTheAv.ViewModels;

namespace ChessOnTheAv.Views;

public partial class MainView : UserControl
{
    private ChessBoardViewModel? _viewModel;

    public MainView()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, System.EventArgs e)
    {
        _viewModel = DataContext as ChessBoardViewModel;
    }

    private void Square_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is Border border && border.DataContext is SquareViewModel square)
        {
            _viewModel?.OnSquareClicked(square);
        }
    }

    private async void NewPosition_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            await _viewModel.LoadNewPosition();
        }
    }

    private void ResetBoard_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.LoadMiddlegamePosition();
        }
    }

    private void Hint_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Implement hint functionality
    }

    private void ExportDebug_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            var debugState = _viewModel.ExportDebugState();
            // TODO: Show debug state in a dialog or copy to clipboard
        }
    }

    private void ImportPgnFile_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Implement file picker for PGN import
    }

    private void ExportCurrentGamePgn_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.ExportCurrentGamePgn();
        }
    }

    private void ClearGamesBank_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.ClearImportedGames();
        }
    }

    private void SaveSettings_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Implement settings save
    }

    private void ResetSettings_Click(object? sender, RoutedEventArgs e)
    {
        // TODO: Implement settings reset
    }

    private void GoToFirstMove_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.GoToFirstMove();
        }
    }

    private void GoToPreviousMove_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.GoToPreviousMove();
        }
    }

    private void GoToNextMove_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.GoToNextMove();
        }
    }

    private void GoToLastMove_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.GoToLastMove();
        }
    }

    private void NewGame_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.CloseGameEndPopup();
            _viewModel.LoadMiddlegamePosition();
        }
    }

    private void ClosePopup_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel != null)
        {
            _viewModel.CloseGameEndPopup();
        }
    }

    private void Instructions_GotFocus(object? sender, GotFocusEventArgs e)
    {
        // Prevent focus on instructions text box
        e.Handled = true;
    }

    private void Instructions_LostFocus(object? sender, RoutedEventArgs e)
    {
        // Prevent focus on instructions text box
    }

    private void Instructions_KeyDown(object? sender, KeyEventArgs e)
    {
        // Prevent keyboard input on instructions text box
        e.Handled = true;
    }
}