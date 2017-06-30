using SimpleGame.Games.FoodEatingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleGame
{
    class FoodEatingGameBoard : IGameState
    {
        public char O = GridConstants.EmptySpaceChar;
        public char F = GridConstants.FoodChar;

        char[][] board = new char[15][];
        char[][] activeBoard = new char[15][];

        public FoodEatingGameBoard(bool isRandom)
        {
            if (isRandom)
            {
                MakeRandom();
            }
            else
            {
                MakeBasic();
            }

            for (int i=0;i<board.Length;i++)
            {
                activeBoard[i] = new char[board[0].Length];
            }

            Reset();
        }

        static FoodEatingGameBoard tempRandomBoard = new FoodEatingGameBoard(true);

        public static FoodEatingGameBoard GetRandomBoard()
        {
            return tempRandomBoard;
        }

        void MakeBasic()
        {
            board[0] = new[] { O, O, O, O, O, O, O, O, O, F };
            board[1] = new[] { F, F, F, F, F, F, F, O, O, O };
            board[2] = new[] { O, O, O, O, O, O, F, O, O, O };
            board[3] = new[] { O, O, F, F, F, F, F, O, O, O };
            board[4] = new[] { O, O, O, O, O, O, F, O, O, O };
            board[5] = new[] { O, O, O, O, O, O, F, F, F, O };
            board[6] = new[] { O, O, O, O, O, O, F, O, O, O };
            board[7] = new[] { O, F, O, O, O, F, F, O, O, O };
            board[8] = new[] { O, O, O, O, O, O, O, O, O, O };
            board[9] = new[] { O, O, O, O, O, O, O, O, O, O };

            board[10] = new[] { F, O, O, O, F, O, O, O, F, O };
            board[11] = new[] { O, F, O, F, O, F, O, F, O, F };
            board[12] = new[] { O, O, F, O, O, O, F, O, O, O };

        }

        void MakeRandom()
        {
            var r = new Random();

            for(int i=0;i<board.Length;i++)
            {
                List<char> row = new List<char>();

                for (int j = 0; j < board.Length; j++)
                {
                    if(r.Next(0,2)==0)
                    {
                        row.Add(GridConstants.EmptySpaceChar);
                    }
                    else
                    {
                        row.Add(GridConstants.FoodChar);
                    }
                }

                board[i] = row.ToArray();
            }
        }

        public ItemAtPoint GetItemAtActiveBoard(int x,int y)
        {
            if(x<0 || y<0 || x>activeBoard[0].Length-1 || y>activeBoard.Length-1)
            {
                return ItemAtPoint.OutOfBounds;
            }

            var charToCheck = activeBoard[y][x];

            return ParseItemAtPoint(charToCheck);
        }

        public void ClearItemAtActiveBoard(int x, int y)
        {
            if (x < 0 || y < 0 || x > activeBoard[0].Length-1 || y > activeBoard.Length-1)
            {
                throw new Exception();
            }

            activeBoard[y][x] = O;
        }

        public void Reset()
        {
            for (int i = 0; i < board[0].Count(); i++)
            {
                for (int j = 0; j < board.Count(); j++)
                {
                    activeBoard[j][i] = board[j][i];
                }
            }
        }

        public ItemAtPoint GetPositionInDirection(Direction direction,int x,int y)
        {
            switch (direction)
            {
                case Direction.Up:
                    return GetItemAtActiveBoard(x, y-1);
                case Direction.Down:
                    return GetItemAtActiveBoard(x, y+1);
                case Direction.Left:
                    return GetItemAtActiveBoard(x-1, y);
                case Direction.Right:
                    return GetItemAtActiveBoard(x+1, y);
                default:
                    throw new Exception();
            }
        }

        public ItemAtPoint ParseItemAtPoint(char c)
        {
            if(c==GridConstants.FoodChar)
            {
                return ItemAtPoint.Food;
            }
            else if(c==GridConstants.EmptySpaceChar)
            {
                return ItemAtPoint.Nothing;
            }

            throw new Exception();
        }

        public void PrintWithPlayerPosition(int x,int y,int numFoodEaten)
        {
            for(int k=0;k<activeBoard[0].Count()+2;k++)
            {
                Console.Write(GridConstants.BorderChar);
            }

            Console.WriteLine();

            for (int i = 0 ; i < activeBoard.Count(); i++)
            {
                Console.Write(GridConstants.BorderChar);

                for (int j = 0; j < activeBoard[0].Count(); j++)
                {
                    if(x==j && y==i)
                    {
                        Console.Write(GridConstants.PlayerChar);
                    }
                    else
                    {
                        Console.Write(activeBoard[i][j]);
                    }
                }

                Console.Write(GridConstants.BorderChar);
                Console.WriteLine();
            }

            for (int k = 0; k < activeBoard[0].Count() + 2; k++)
            {
                Console.Write(GridConstants.BorderChar);
            }

            Console.WriteLine();
            Console.WriteLine(numFoodEaten);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (char[] line in board)
            {
                foreach (char point in line)
                {
                    sb.Append(point);
                }

                sb.Append('\n');
            }

            return sb.ToString();
        }
    }
}
