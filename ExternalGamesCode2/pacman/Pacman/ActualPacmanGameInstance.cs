using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacman
{
    public class ActualPacmanGameInstance
    {
        public static int LengthOfDataToSend = 32; //40

        public static bool ARE_GHOSTS_ACTIVE = true;

        public static int DEATH_PENALTY = 0;
        public static int TIMER_TICK_PENALTY = 0;
        public static int WALL_BUMPING_PENALTY = 0;
        public static int OLD_POSITION_PENALTY = 0;

        public static int GHOST_EATING_SCORE = 5; // 50/300
        public static int GHOST_SEEING_SCORE = 0;
        public static int FOOD_SCORE = 1; //10
        public static int SUPER_FOOD_SCORE = 1; //50
        public static int NEW_POSITION_SCORE = 0;

        public static int NUMLIVES = 1;//3

        public static ActualPacmanGameInstance Instance;

        public static bool isLogging = false;
        public static bool IS_HEADLESS;
        public static int maxtimerMax = 10000; //3000
        public static int maxTimer = maxtimerMax;

        private int _randomSeed;

        public static void SetScoresToDefault()
        {
            ActualPacmanGameInstance.DEATH_PENALTY = 0;
            ActualPacmanGameInstance.TIMER_TICK_PENALTY = 0;
            ActualPacmanGameInstance.WALL_BUMPING_PENALTY = 0;
            ActualPacmanGameInstance.OLD_POSITION_PENALTY = 0;

            ActualPacmanGameInstance.GHOST_EATING_SCORE = 50;
            ActualPacmanGameInstance.GHOST_SEEING_SCORE = 0;
            ActualPacmanGameInstance.FOOD_SCORE = 10;
            ActualPacmanGameInstance.SUPER_FOOD_SCORE = 50;
            ActualPacmanGameInstance.NEW_POSITION_SCORE = 0;
        }

        public static bool IS_GAME_OVER { get; private set; } = false;
        public static void EndGame()
        {
            IS_GAME_OVER = true;

            if(!IS_HEADLESS)
            {
                Thread.Sleep(5000);
                Environment.Exit(0);
            }
        }

        //private ILog _logger;

        public static void SetUpInstance(bool isHeadless,int randomSeed)
        {
            IS_HEADLESS = isHeadless;
            Instance = new ActualPacmanGameInstance(randomSeed);
            IS_GAME_OVER = false;

            if(!IS_HEADLESS)
            {
                sb = new StringBuilder();
            }
            else
            {
                Instance.food.InitializeFood();
            }

            maxTimer = maxtimerMax;
        }


        static StringBuilder sb;
       
        public static void Log(String s)
        {
            if(!IS_HEADLESS)
            {
                sb.AppendLine(s);
            }
            else if(isLogging)
            {
                Console.WriteLine(s);
            }
        }

        public ActualPacmanGameInstance(int randomSeed)
        {
            _randomSeed = randomSeed;

            gameboard = new GameBoard();
            food = new Food();
            pacman = new Pacman();
            ghost = new Ghost(randomSeed);
            player = new Player();
            highscore = new HighScore(randomSeed);
        }

        public void Tick()
        {
            try
            {
                //_logger.Debug("Began tick");

                bool showHeadless = false;
                var direction = -1;

                if (pacman.moveQueue.Any())
                {
                    direction = pacman.moveQueue.Peek();
                    //_logger.Debug("Found direction: " + direction);
                }

                pacman.Tick();

                if(ARE_GHOSTS_ACTIVE)
                {
                    ghost.Tick();
                }

                if (IS_HEADLESS && showHeadless)
                {
                    PrintGameToConsole(direction);
                }

                if (maxTimer-- < 0 )//|| NoScoreCount > NoScoreTimeOut)
                {
                    //_logger.Debug("Ending game due to timer running out.");
                    EndGame();
                }

                if(TIMER_TICK_PENALTY != 0)
                {
                    player.UpdateScore(-TIMER_TICK_PENALTY);
                }

                //_logger.Debug($"Ended tick. Lives: {player.Lives} Score: {player.Score} Pacman Position: {pacman.xCoordinate},{pacman.yCoordinate}");
            }
            catch(Exception e)
            {
                //_logger.Debug(e);
            }
        }

        public static int NoScoreCount = 0;
        public static int NoScoreTimeOut = 10;

        public GameBoard gameboard;
        public Food food;
        public Pacman pacman;
        public Ghost ghost;
        public Player player;
        public HighScore highscore;

        public void SendInput(int k)
        {
            string directionName = "Undefined";
            switch(k)
            {
                case 0:
                    directionName = "Up";
                    break;
                case 1:
                    directionName = "Down";
                    break;
                case 2:
                    directionName = "Left";
                    break;
                case 3:
                    directionName = "Right";
                    break;
            }

            //Log("Recieved " + directionName);

            try
            {
                Instance.pacman.moveQueue.Enqueue(k);
                Tick();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public int[] GetStatus()
        {
            if (IS_GAME_OVER)
            {
                var score = Instance.player.Score;
                var lowerScore = (score / 255);
                if (lowerScore > 255)
                {
                    lowerScore = 255;
                }

                int[] toSend = new int[LengthOfDataToSend];
                toSend[0] = 100;
                toSend[1] = (byte)(score % 255);
                toSend[2] = (byte)(score / 255);

                //Log("Sending game over state: " + toSend.Select(i => i.ToString()).Aggregate((i, j) => (i + " " + j)));

                if(!IS_HEADLESS)
                {
                    var sw = new StreamWriter(@"C:\ProjectLogs\PacmanLogs\PacmanBetaLog" + Process.GetCurrentProcess().Id + ".txt");
                    sw.Write(sb.ToString());
                    sw.Close();

                    new Task(() => Application.Exit()).Start();
                }

                return toSend;
            }

            int x = Instance.pacman.xCoordinate;
            int y = Instance.pacman.yCoordinate;
            var board = Instance.gameboard.Matrix;

            //int[] pellets = new int[4];

            /*lock(ActualPacmanGameInstance.Instance)
            {
                pellets = Instance.gameboard.GetPelletDirections(x, y);
            }*/

            var status = new List<int>()
            {
                GetFromBoard(board,x-8,y), //NO
                GetFromBoard(board,x+8,y),
                GetFromBoard(board,x,y-8),
                GetFromBoard(board,x,y+8),

                GetFromBoard(board,x-7,y), //NO
                GetFromBoard(board,x+7,y),
                GetFromBoard(board,x,y-7),
                GetFromBoard(board,x,y+7),

                GetFromBoard(board,x-6,y), 
                GetFromBoard(board,x+6,y),
                GetFromBoard(board,x,y-6),
                GetFromBoard(board,x,y+6),

                GetFromBoard(board,x-5,y),
                GetFromBoard(board,x+5,y),
                GetFromBoard(board,x,y-5),
                GetFromBoard(board,x,y+5),

                GetFromBoard(board,x-4,y),
                GetFromBoard(board,x+4,y),
                GetFromBoard(board,x,y-4),
                GetFromBoard(board,x,y+4),

                GetFromBoard(board,x-3,y),
                GetFromBoard(board,x+3,y),
                GetFromBoard(board,x,y-3),
                GetFromBoard(board,x,y+3),

                GetFromBoard(board,x-2,y), //NO
                GetFromBoard(board,x+2,y),
                GetFromBoard(board,x,y-2),
                GetFromBoard(board,x,y+2),

                GetFromBoard(board,x-1,y), //NO
                GetFromBoard(board,x+1,y),
                GetFromBoard(board,x,y-1),
                GetFromBoard(board,x,y+1),

                /*GetFromBoard(board, x-2, y-2),
                GetFromBoard(board, x-1, y-2),
                GetFromBoard(board, x, y-2),
                GetFromBoard(board, x+1, y-2),
                GetFromBoard(board, x+2, y-2),

                GetFromBoard(board, x-2, y-1),
                GetFromBoard(board, x-1, y-1),
                GetFromBoard(board, x, y-1),
                GetFromBoard(board, x+1, y-1),
                GetFromBoard(board, x+2, y-1),

                GetFromBoard(board, x-2, y),
                GetFromBoard(board, x-1, y),
                GetFromBoard(board, x+1, y),
                GetFromBoard(board, x+2, y),

                GetFromBoard(board, x-2, y+1),
                GetFromBoard(board, x-1, y+1),
                GetFromBoard(board, x, y+1),
                GetFromBoard(board, x+1, y+1),
                GetFromBoard(board, x+2, y+1),

                GetFromBoard(board, x-2, y+2),
                GetFromBoard(board, x-1, y+2),
                GetFromBoard(board, x, y+2),
                GetFromBoard(board, x+1, y+2),
                GetFromBoard(board, x+2, y+2),*/

            }.ToArray();


            /*var top1 = GetFromBoard(board, x, y-1);
            var top2 = GetFromBoard(board, x, y-2);

            var left1 = GetFromBoard(board, x - 1, y);
            var left2 = GetFromBoard(board, x - 2, y);

            var bottom1 = GetFromBoard(board, x, y + 1);
            var bottom2 = GetFromBoard(board, x, y + 2);

            var right1 = GetFromBoard(board, x + 1, y);
            var right2 = GetFromBoard(board, x + 2, y);*/




            //int[] status = { top1, top2, left1, left2, bottom1, bottom2, right1, right2 };

            //int[] status = view;
            

            //Log("Sending game state: " + status.Select(i => i.ToString()).Aggregate((i, j) => (i + " " + j)) + " Score: " + player.Score + " Lives: " + player.Lives);

            return status;
        }

        public static int[] GetPelletsOnLine(int[,] board,int x,int y)
        {
            int[] pellets = new int[4];


            for(int i=0;i<30;i++)
            {
                if(GetFromBoard(board,x,y+i)==1)
                {
                    pellets[0] = 1;
                }
            }

            for (int i = 0; i < 30; i++)
            {
                if (GetFromBoard(board, x, y - i) == 1)
                {
                    pellets[1] = 1;
                    break;
                }
            }

            for (int i = 0; i < 30; i++)
            {
                if (GetFromBoard(board, x + i,y) == 1)
                {
                    pellets[2] = 1;
                    break;
                }
            }

            for (int i = 0; i < 30; i++)
            {
                if (GetFromBoard(board, x - i,y) == 1)
                {
                    pellets[3] = 1;
                    break;
                }
            }

            return pellets;
        }

        public static byte GetFromBoard(int[,] board, int x, int y)
        {
            if (x < 0 || y < 0 || x > 29 || y > 26)
            {
                return 99;
            }

            if(ARE_GHOSTS_ACTIVE)
            {
                var ghostValue = GhostCheck(board, x, y);

                if (ghostValue != 0)
                {
                    return (byte)ghostValue;
                }
            }

            var item = board[x, y];

            return (byte)item;
        }

        public static int GhostCheck(int[,] board, int x, int y)
        {
            for(int i=0;i<4;i++)
            {
                if(Instance.ghost.xCoordinate[i] == x && Instance.ghost.yCoordinate[i] == y)
                {
                    if (GHOST_SEEING_SCORE != 0)
                    {
                        Instance.player.UpdateScore(GHOST_SEEING_SCORE);
                    }

                    if (Instance.ghost.State[i] == 0)
                    {
                        return 66;
                    }
                    else if (Instance.ghost.State[i] == 1)
                    {
                        return 67;
                    }
                }
            }

            return 0;
        }


        public void PrintGameToConsole(int dir)
        {
            var matrix = gameboard.Matrix;

            for(int i = 0;i<30;i++)
            {
                for(int j=27;j>0;j--)
                {
                    var m = matrix[i, j];

                    if (j == pacman.xCoordinate && i == pacman.yCoordinate)
                    {
                        Console.Write('#');
                    }
                    else if (j == ghost.xCoordinate[0] && i == ghost.yCoordinate[0] ||
                        j == ghost.xCoordinate[1] && i == ghost.yCoordinate[1] ||
                        j == ghost.xCoordinate[2] && i == ghost.yCoordinate[2] ||
                        j == ghost.xCoordinate[3] && i == ghost.yCoordinate[3])
                    {
                        Console.Write('G');
                    }
                    else if (m == 10)
                    {
                        Console.Write('w');
                    }
                    else if (m == 0)
                    {
                        Console.Write(' ');
                    }
                    else if (m == 1)
                    {
                        Console.Write('.');
                    }
                    else if (m == 2)
                    {
                        Console.Write('0');
                    }
                    else if (m == 0)
                    {
                        Console.Write(' ');
                    }
                    else
                    {
                        Console.Write('?');
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("Lives: " + player.Lives);
            Console.WriteLine("Score: " + player.Score);
            Console.WriteLine("Direction moved: " + dir);


            Console.ReadLine();
            Console.Clear();
        }
    }
}
