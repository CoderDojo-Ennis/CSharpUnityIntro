using System;
using System.Collections.Generic;

class Game
{
    public Ball ball;

    public void StartGame()
    {
        CreateBall();
        RunGameLoop();
    }

    public void CreateBall()
    {
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
        ball.Move();
        ball.Draw();
    }

}
