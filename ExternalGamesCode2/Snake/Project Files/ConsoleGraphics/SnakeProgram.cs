using System;
using System.Drawing;
using System.Windows;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleGraphics
{

    //Snake C# Implementation created by Finley Warman. Available at https://github.com/FinWarman/Snake

    public class SnakeProgram
    {
        public bool isToScoreMode = true;
        public int scoreToAimFor = 100;

        public bool IsHeadless;
        public bool IsNonKeyboard;

        public static int screenWidth = 40; //56
        public static int screenHeight = 33; //38

        public static int maxNoEatingTime = screenHeight*screenWidth;

        public List<int> inputBuffer;
        int score;
        int x;
        int y;
        int colourTog;
        bool alive;
        bool pelletOn;
        int pelletX;
        int pelletY;
        int currentNoEatingTimerTicks;
        int currentTotalTimerTicks;

        int[] xPoints;
        int[] yPoints;
        int delay;
        string direction;
        int snakeLength;
        Random rnd;

        ConsoleColor bgColor;
        ConsoleColor fgColor;
        ConsoleColor tailColor;

        public void Tick()
        {
            if (pelletOn == false)
            {
                pelletStart:

                bool collide = false;
                pelletOn = true;
                pelletX = rnd.Next(4, screenWidth - 4);
                pelletY = rnd.Next(4, screenHeight - 4);

                for (int l = (xPoints.Length - 1); l > 1; l--)
                {
                    if (xPoints[l] == pelletX & yPoints[l] == pelletY)
                    {
                        collide = true;
                    }
                }
                if (collide == true)
                {
                    goto pelletStart;
                }
                else
                {
                    if(!IsHeadless)
                    {
                        Console.SetCursorPosition(pelletX, pelletY);
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.BackgroundColor = bgColor;
                        Console.Write("#");
                    }
                    pelletOn = true;
                }

            }

            Array.Resize<int>(ref xPoints, snakeLength);
            Array.Resize<int>(ref yPoints, snakeLength);

            colourTog++;

            if(IsNonKeyboard)
            {
                if(inputBuffer.Count!=1)
                {
                    throw new Exception();
                }
                else
                {
                    var key = inputBuffer.First();
                    inputBuffer.Clear();

                    if(key==0 && direction != "down")
                    {
                        direction = "up";

                    }
                    if (key == 1 && direction != "up")
                    {
                        direction = "down";

                    }
                    if (key == 2 && direction != "right")
                    {
                        direction = "left";

                    }
                    if (key == 3 && direction != "left")
                    {
                        direction = "right";
                    }
                }
            }
            else if (Console.KeyAvailable)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (direction != "left")
                        {
                            direction = "right";
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (direction != "right")
                        {
                            direction = "left";
                        }
                        break;
                    case ConsoleKey.UpArrow:

                        if (direction != "down")
                        {
                            direction = "up";
                        }
                        break;
                    case ConsoleKey.DownArrow:

                        if (direction != "up")
                        {
                            direction = "down";
                        }
                        break;
                    default:
                        break;
                }
            } //Inputs & direction


            if (direction == "right")
            {
                x += 1;
            }
            else if (direction == "left")
            {
                x -= 1;
            }
            else if (direction == "down")
            {
                y += 1;
            }
            else if (direction == "up")
            {
                y -= 1;
            }

            xPoints[0] = x;
            yPoints[0] = y;

            for (int l = (xPoints.Length - 1); l > 0; l--)
            {
                xPoints[l] = xPoints[l - 1];
                yPoints[l] = yPoints[l - 1];
            }

            if(x<0 || y<0 || x>=screenWidth || y>=screenHeight)
            {
                alive = false;
                return;
            }

            if(!IsHeadless)
            {
                try
                {
                    Console.SetCursorPosition(xPoints[0], yPoints[0]);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    alive = false;
                }

                if (colourTog == 2)
                {
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                }
                else
                {
                    colourTog = 1;
                    Console.BackgroundColor = ConsoleColor.Green;
                }
                Console.ForegroundColor = fgColor;
                Console.Write("*");

                try
                {
                    Console.SetCursorPosition(xPoints[xPoints.Length - 1], yPoints[yPoints.Length - 1]);
                }
                catch (System.ArgumentOutOfRangeException)
                {
                    alive = false;
                }
                Console.BackgroundColor = tailColor;
                Console.Write(" ");

            }


            if (x == pelletX & y == pelletY)
            {
                pelletOn = false;
                snakeLength += 1;
                delay -= delay / 16;
                currentNoEatingTimerTicks = 0;

                if(!IsHeadless)
                {
                    new Thread(() => Console.Beep(320, 250)).Start();
                }
            }

            for (int l = (xPoints.Length - 1); l > 1; l--)
            {
                if (xPoints[l] == xPoints[0] & yPoints[l] == yPoints[0])
                {
                    alive = false;
                }

            }
            score = ((snakeLength) - 8);

            if(!IsHeadless)
            {
                Console.SetCursorPosition(2, 2);
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write("Score: {0} ", score);
            }

            currentNoEatingTimerTicks++;
            currentTotalTimerTicks++;

            if(currentNoEatingTimerTicks>maxNoEatingTime)
            {
                alive = false;
            }

            if (isToScoreMode)
            {
                if (score >= scoreToAimFor)
                {
                    score += ((256*256)-currentTotalTimerTicks);
                    alive = false;
                }
            }

        }



        public void Setup(bool isHeadless,bool isNonKeyboard,int randomSeed)
        {
            IsHeadless = isHeadless;
            IsNonKeyboard = isNonKeyboard;

            if(isNonKeyboard)
            {
                inputBuffer = new List<int>();
            }

            score = 0;
            x = 2;
            y = 2;
            colourTog = 1;
            alive = true;
            pelletOn = false;
            pelletX = 0;
            pelletY = 0;
            xPoints = new int[8] { 20, 19, 18, 17, 16, 15, 14, 13 };
            yPoints = new int[8] { 20, 20, 20, 20, 20, 20, 20, 20 };
            delay = 100;
            direction = "right";
            snakeLength = 8;
            rnd = new Random(randomSeed);
            currentNoEatingTimerTicks = 0;
            currentTotalTimerTicks = 0;

            if (!isHeadless)
            {
                Console.CursorVisible = (false);
                Console.Title = "Snaaaaake!";
                Console.SetWindowSize(screenWidth, screenHeight);
                //Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                bgColor = Console.BackgroundColor;
                fgColor = Console.ForegroundColor;
                tailColor = Console.BackgroundColor;
            }
        }

        static void Main(string[] args)
        {
            var program = new SnakeProgram();

            Game:
            program.Setup(false,false,1);

            while (program.alive)
            {
                program.Tick();
                Thread.Sleep(program.delay);
            }

            new Thread(() => Console.Beep(37, 1)).Start();
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            //Console.Beep(831, 250);
            //Console.Beep(785, 250);

            ConsoleColor[] colors = (ConsoleColor[])ConsoleColor.GetValues(typeof(ConsoleColor));

            for (int i = 0; i < 1; i++)
            {
                foreach (var color in colors)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.ForegroundColor = color;
                    Console.Clear();
                    Console.WriteLine("\n\n\n\n\n");
                    Console.WriteLine("\n                       Game over :(");
                    Console.WriteLine("\n\n                   Your score was: {0} !", program.score);
                    System.Threading.Thread.Sleep(100);
                }
            }
            Thread.Sleep(1000);
            Console.WriteLine("\n\n\n\n\n\n             -- Press Any Key To Continue --");
            Thread.Sleep(500);
            Console.ReadKey(true);
            Console.ReadKey(true);
            goto Game;
        }

        public void SendInput(int d)
        {
            inputBuffer.Add(d);
        }

        public int[] GetStatus()
        {
            if(!alive)
            {
                var lowerScore = (score / 255);
                if (lowerScore > 255)
                {
                    lowerScore = 255;
                }

                var scoreLower = (score % 255);
                var scoreHigher = (score / 255);

                return new int[] { 254, scoreLower, scoreHigher};
            }

            var beside1 = ItemAtPoint(0, -1);
            var beside2 = ItemAtPoint(0, 1);
            var beside3 = ItemAtPoint(-1, 0);
            var beside4 = ItemAtPoint(1, 0);

            var beside5 = ItemAtPoint(0, -2);
            var beside6 = ItemAtPoint(0, 2);
            var beside7 = ItemAtPoint(-2, 0);
            var beside8 = ItemAtPoint(2, 0);


            var horizontalFoodPosition = GetRelativeHorizontalFoodPosition();
            var verticalFoodPosition = GetRelativeVerticalFoodPosition();

            return new[] { beside1, beside2, beside3, beside4, beside5,beside6,beside7,beside8 ,horizontalFoodPosition, verticalFoodPosition };
        }

        private int ItemAtPoint(int relX,int relY)
        {
            var xToSearch = relX + x;
            var yToSearch = relY + y;

            if (xToSearch < 0 || yToSearch < 0 || xToSearch >= screenWidth || yToSearch >= screenHeight)
            {
                return 0; //out of bounds
            }
            else if(xToSearch==pelletX && yToSearch==pelletY)
            {
                return 1; // pellet
            }
            else if(PointIsInSnake(xToSearch,yToSearch))
            {
                return 2; //snake
            }

            return 3; //nothing
        }

        private bool PointIsInSnake(int pX,int pY)
        {
            for (int i = 0; i < xPoints.Length; i++)
            {
                if (pX == xPoints[i] && pY == yPoints[i])
                {
                    return true;
                }
            }

            return false;
        }

        private int GetRelativeHorizontalFoodPosition()
        {
            if(pelletX>x)
            {
                return 0;
            }
            else if(pelletX<x)
            {
                return 1;
            }

            return 2;
        }

        private int GetRelativeVerticalFoodPosition()
        {
            if (pelletY > y)
            {
                return 0;
            }
            else if (pelletY < y)
            {
                return 1;
            }

            return 2;
        }
    }
}
