using System;

class Ball
{
    int X;
    int Y;
    int XSpeed = 1;
    int YSpeed = 1;
    ConsoleColor BallColor;

    public Ball(ConsoleColor color, int x, int y)
    {
        BallColor = color;
        X = x;
        Y = y;
    }

    public void Move()
    {
        X = X + XSpeed;
        Y = Y + YSpeed;

        BounceOnEdge();
    }

    private void BounceOnEdge()
    {
        if (X == 0 || X == (Console.WindowWidth - 1))
        {
            XSpeed = XSpeed * -1;
        }
        if (Y == 0 || Y == (Console.WindowHeight - 1))
        {
            YSpeed = YSpeed * -1;
        }
    }

    public void Draw()
    {
        Console.ForegroundColor = BallColor;
        Console.CursorLeft = X;
        Console.CursorTop = Y;
        Console.Write("*");
    }
}
