using System;
using System.Linq;
using System.Text;

namespace SimpleGame
{
    class GameBoard
    {
        public char O = GridConstants.EmptySpaceChar;
        public char F = GridConstants.FoodChar;

        char[][] board = new char[10][];
        char[][] activeBoard = new char[10][];

        public GameBoard()
        {
            MakeBasic();
            ResetBoard();
        }

        void MakeBasic()
        {
            board[0] = new[] { O, O, O, O, O, O, O, O, O, O };
            board[1] = new[] { F, F, F, F, F, F, F, O, O, O };
            board[2] = new[] { O, O, O, O, O, O, F, O, O, O };
            board[3] = new[] { O, O, O, O, O, O, F, O, O, O };
            board[4] = new[] { O, O, O, O, O, O, F, O, O, O };
            board[5] = new[] { O, O, O, O, O, O, F, O, O, O };
            board[6] = new[] { O, O, O, O, O, O, F, O, O, O };
            board[7] = new[] { O, O, O, O, O, F, F, O, O, O };
            board[8] = new[] { O, O, O, O, O, O, O, O, O, O };
            board[9] = new[] { O, O, O, O, O, O, O, O, O, O };
        }

        public ItemAtPoint GetItemAtActiveBoard(int x,int y)
        {
            if(x<0 || y<0 || x>9 || y>9)
            {
                return ItemAtPoint.OutOfBounds;
            }

            var charToCheck = activeBoard[y][x];

            return ParseItemAtPoint(charToCheck);
        }

        public void ClearItemAtActiveBoard(int x, int y)
        {
            if (x < 0 || y < 0 || x > 9 || y > 9)
            {
                throw new Exception();
            }

            activeBoard[y][x] = O;
        }

        public void ResetBoard()
        {
            activeBoard = (char[][])board.Clone();
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
            for(int k=0;k<activeBoard.Count()+2;k++)
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
                        Console.Write('P');
                    }
                    else
                    {
                        Console.Write(activeBoard[i][j]);
                    }
                }

                Console.Write(GridConstants.BorderChar);
                Console.WriteLine();
            }

            for (int k = 0; k < activeBoard.Count() + 2; k++)
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
