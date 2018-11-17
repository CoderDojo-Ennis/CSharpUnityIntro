using System;

class Ball
{
    int X;
    int Y;
    int XSpeed = 1;
    int YSpeed = 1;
    ConsoleColor BallColor;

    public Ball(int startX, int startY, ConsoleColor startColor)
    {
        X = startX;
        Y = startY;
        BallColor = startColor;
    }

    public void Move()
    {
        X = X + XSpeed;
        Y = Y + YSpeed;

        IfOnEdgeBounce();
    }

    private void IfOnEdgeBounce()
    {
        if (X == 0 || X == (Console.WindowWidth - 1))
        {
            XSpeed = XSpeed * -1;
        }
        if (Y == 0 || Y == Console.WindowHeight)
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
