using System;
using System.Collections.Generic;

class Game
{
    public List<Ball> Balls;
    public Ball ball;

    public void StartGame()
    {
        CreateBall();
        RunGameLoop();
    }

    public void CreateBall()
    {
        Balls = new List<Ball>();
        Balls.Add(new Ball(ConsoleColor.Yellow, 3, 3));
        Balls.Add(new Ball(ConsoleColor.Red, 15, 10));
        Balls.Add(new Ball(ConsoleColor.Cyan, 1, 1));
        Balls.Add(new Ball(ConsoleColor.White, 5, 8));
        Balls.Add(new Ball(ConsoleColor.Green, 20, 12));
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

            DrawScreen();

            System.Threading.Thread.Sleep(20);

        }
    }


    public void ClearScreen()
    {
        Console.Clear();
        Console.CursorVisible = false;
    }

    public void DrawScreen()
    {
        foreach(var ball in Balls)
        {
            ball.Move();
            ball.Draw();
        }
    }

}
