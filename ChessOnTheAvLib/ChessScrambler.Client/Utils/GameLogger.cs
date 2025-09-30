using System;

namespace ChessScrambler.Client.Utils
{
    public static class GameLogger
    {
        public static bool EnableGameLogging { get; set; } = false;

        public static void Log(string message)
        {
            if (EnableGameLogging)
            {
                Console.WriteLine(message);
            }
        }

        public static void LogGame(string message)
        {
            if (EnableGameLogging)
            {
                Console.WriteLine($"[GAME] {message}");
            }
        }
    }
}
