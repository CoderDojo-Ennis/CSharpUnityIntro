using System;

class Program
{
    static void Main()
    {
        Game game = new Game();

        game.StartGame();

        Console.WriteLine("Game Over");
        Console.ReadKey();
        Console.CursorVisible = true;
    }

    public static bool WasEscapePressed()
    {
        if (Console.KeyAvailable)
        {
            var keyInfo = Console.ReadKey();
            if (keyInfo.Key == ConsoleKey.Escape)
            {
                return true;
            }
        }

        return false;
    }

}
