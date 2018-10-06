using System;

class Ball
{
    int X;
    int Y;
    int XSpeed = 1;
    int YSpeed = 1;
    ConsoleColor BallColor;

    public Ball()
    {
    }

    public void Move()
    {
    }

    public void Draw()
    {
        Console.ForegroundColor = BallColor;
        Console.CursorLeft = X;
        Console.CursorTop = Y;
        Console.Write("*");
    }
}
