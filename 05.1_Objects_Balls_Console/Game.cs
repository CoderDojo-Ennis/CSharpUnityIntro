using System;
using System.Collections.Generic;

class Game
{
    List<Ball> Balls;

    public void StartGame()
    {
        Balls = new List<Ball>();
        CreateBall();
        RunGameLoop();
    }

    public void CreateBall()
    {
        for(int i = 0; i< 10; i = i + 1)
        {
            int x = new Random().Next(1, 20);
            int y = new Random().Next(1, 20);
            Ball ball = new Ball(x, y, ConsoleColor.Green);
            Balls.Add(ball);
        }
    }

    public void RunGameLoop()
    {
        bool done = false;

        while (done == false)
        {
            if (Program.WasEscapePressed())
            {
                done = true;
            }

            ClearScreen();
            DrawScreen();

            System.Threading.Thread.Sleep(40);

        }
    }

    public void ClearScreen()
    {
        Console.Clear();
        Console.CursorVisible = false;
    }

    public void DrawScreen()
    {
        foreach (Ball ball in Balls)
        {
            ball.Move();
            ball.Draw();
        }
    }

}
