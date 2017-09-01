using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleGame.Games.FoodEatingGame
{
    class FoodEatingGameBoard : IGameState
    {
        private const int defaultHeight = 20;
        private const int defaultWidth = 20;

        private char O = GridConstants.EmptySpaceChar;
        private char F = GridConstants.FoodChar;

        private char[][] _baseGrid = new char[defaultHeight][];
        private char[][] _activeGrid = new char[defaultWidth][];

        protected FoodEatingGameBoard(bool isRandom)
        {
            if (isRandom)
            {
                _baseGrid = GetRandomGrid();
            }
            else
            {
                _baseGrid = LoadGridFromFile();
            }

            for (int i=0;i<_baseGrid.Length;i++)
            {
                _activeGrid[i] = new char[_baseGrid[0].Length];
            }

            Reset();
        }
        
        private char[][] LoadGridFromFile()
        {
            throw new NotImplementedException();
        }

        private char[][] GetRandomGrid()
        {
            char[][] randomGrid = new char[defaultHeight][];
            var r = new Random();

            for(int i=0;i< defaultHeight;i++)
            {
                List<char> row = new List<char>();

                for (int j = 0; j < defaultWidth; j++)
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

                randomGrid[i] = row.ToArray();
            }

            return randomGrid;
        }

        public static FoodEatingGameBoard GetRandomBoard()
        {
            return new FoodEatingGameBoard(isRandom: true);
        }

        public ItemAtPoint GetItemAtActiveBoard(int x,int y)
        {
            if(x<0 || y<0 || x>_activeGrid[0].Length-1 || y>_activeGrid.Length-1)
            {
                return ItemAtPoint.OutOfBounds;
            }

            var charToCheck = _activeGrid[y][x];

            return ParseItemAtPoint(charToCheck);
        }

        public void ClearItemAtActiveBoard(int x, int y)
        {
            if (x < 0 || y < 0 || x > _activeGrid[0].Length-1 || y > _activeGrid.Length-1)
            {
                throw new Exception();
            }

            _activeGrid[y][x] = O;
        }

        public void Reset()
        {
            try
            {
                for (int i = 0; i < _baseGrid[0].Count(); i++)
                {
                    for (int j = 0; j < _baseGrid.Count(); j++)
                    {
                        _activeGrid[j][i] = _baseGrid[j][i];
                    }
                }
            }
            catch(Exception)
            {
                Console.WriteLine("Board arrays probably out of bounds.");
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

        public ItemAtPoint GetPositionInDirection(Direction direction, int x, int y)
        {
            switch (direction)
            {
                case Direction.Up:
                    return GetItemAtActiveBoard(x, y - 1);
                case Direction.Down:
                    return GetItemAtActiveBoard(x, y + 1);
                case Direction.Left:
                    return GetItemAtActiveBoard(x - 1, y);
                case Direction.Right:
                    return GetItemAtActiveBoard(x + 1, y);
                default:
                    throw new Exception();
            }
        }

        public void PrintWithPlayerPosition(int x,int y,int numFoodEaten)
        {
            for(int k=0;k<_activeGrid[0].Count()+2;k++)
            {
                Console.Write(GridConstants.BorderChar);
            }

            Console.WriteLine();

            for (int i = 0 ; i < _activeGrid.Count(); i++)
            {
                Console.Write(GridConstants.BorderChar);

                for (int j = 0; j < _activeGrid[0].Count(); j++)
                {
                    if(x==j && y==i)
                    {
                        Console.Write(GridConstants.PlayerChar);
                    }
                    else
                    {
                        Console.Write(_activeGrid[i][j]);
                    }
                }

                Console.Write(GridConstants.BorderChar);
                Console.WriteLine();
            }

            for (int k = 0; k < _activeGrid[0].Count() + 2; k++)
            {
                Console.Write(GridConstants.BorderChar);
            }

            Console.WriteLine();
            Console.WriteLine(numFoodEaten);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (char[] line in _baseGrid)
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
