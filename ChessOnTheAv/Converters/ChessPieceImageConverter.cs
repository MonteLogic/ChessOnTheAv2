using System;
using System.Globalization;
using System.IO;
using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ChessOnTheAv.Models;

namespace ChessOnTheAv.Converters;

public class ChessPieceImageConverter : IValueConverter
{
    public static readonly ChessPieceImageConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is ChessPiece piece)
        {
            try
            {
                // Get the resource name for the piece
                var resourceName = GetResourceName(piece);
                
                // Try to load from wwwroot first (for browser), then fallback to avares
                try
                {
                    var wwwrootUri = new Uri($"avares://ChessOnTheAv/wwwroot/Assets/ChessPieces/{resourceName}");
                    return new Bitmap(AssetLoader.Open(wwwrootUri));
                }
                catch
                {
                    // Fallback to Assets folder
                    var uri = new Uri($"avares://ChessOnTheAv/Assets/ChessPieces/{resourceName}");
                    return new Bitmap(AssetLoader.Open(uri));
                }
            }
            catch (Exception ex)
            {
                // Log the error for debugging
                Console.WriteLine($"[ERROR] Failed to load chess piece image: {ex.Message}");
                // Fallback to null if image loading fails
                return null;
            }
        }
        
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    private static string GetResourceName(ChessPiece piece)
    {
        var colorPrefix = piece.Color == PieceColor.White ? "w" : "b";
        var pieceLetter = piece.Type switch
        {
            PieceType.Pawn => "P",
            PieceType.Rook => "R",
            PieceType.Knight => "N",
            PieceType.Bishop => "B",
            PieceType.Queen => "Q",
            PieceType.King => "K",
            _ => "P"
        };
        
        return $"{colorPrefix}{pieceLetter}.png";
    }
}
