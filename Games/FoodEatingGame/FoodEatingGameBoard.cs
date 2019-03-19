using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleGame.Games.FoodEatingGame
{
    class FoodEatingGameBoard : IDiscreteGameState
    {
        private const int defaultHeight = 20;
        private const int defaultWidth = 20;

        private char O = GridConstants.EmptySpaceChar;
        private char F = GridConstants.FoodChar;

        private char[][] _baseGrid = new char[defaultHeight][];
        private char[][] _activeGrid = new char[defaultWidth][];

        private int? _playerX = 0;
        private int? _playerY = 0;

        private bool _foodEaten = false;

        private static int _currentRandomSeed;
        private static FoodEatingGameBoard _currentBoard;

        protected FoodEatingGameBoard(bool isRandom,int randomSeed = 0)
        {
            if (isRandom)
            {
                _baseGrid = GetRandomGrid(randomSeed);
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

        private char[][] GetRandomGrid(int randomSeed)
        {
            char[][] randomGrid = new char[defaultHeight][];
            var r = new Random(randomSeed);

            for(int i=0;i< defaultHeight;i++)
            {
                List<char> row = new List<char>();

                for (int j = 0; j < defaultWidth; j++)
                {
                    if(r.Next(0,3)==0)
                    {
                        row.Add(GridConstants.FoodChar);
                    }
                    else
                    {
                        row.Add(GridConstants.EmptySpaceChar);
                    }
                }

                randomGrid[i] = row.ToArray();
            }

            return randomGrid;
        }

        public static FoodEatingGameBoard GetRandomBoard(int randomSeed)
        {
            if(randomSeed == _currentRandomSeed  && _currentBoard!=null)
            {
                _currentBoard.Reset();
            }
            else
            {
                _currentRandomSeed = randomSeed;
                _currentBoard = new FoodEatingGameBoard(true, randomSeed);
            }

            return _currentBoard;

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

                _playerX = 0;
                _playerY = 0;
                _foodEaten = false;
            }
            catch(Exception)
            {
                Console.WriteLine("Board arrays probably out of bounds.");
            }
        }

        public void Dispose()
        {
            //Nothing to dispose
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

        public int[] GetPlayerData()
        {
            var pX = _playerX.Value;
            var pY = _playerY.Value;

            var top = (int)GetItemAtActiveBoard(pX, pY - 1);
            var bottom = (int)GetItemAtActiveBoard(pX, pY + 1);
            var left = (int)GetItemAtActiveBoard(pX - 1, pY);
            var right = (int)GetItemAtActiveBoard(pX + 1, pY);

            var top2 = (int)GetItemAtActiveBoard(pX, pY - 2);
            var bottom2 = (int)GetItemAtActiveBoard(pX, pY + 2);
            var left2 = (int)GetItemAtActiveBoard(pX - 2, pY);
            var right2 = (int)GetItemAtActiveBoard(pX + 2, pY);

            var top3 = (int)GetItemAtActiveBoard(pX, pY - 3);
            var bottom3 = (int)GetItemAtActiveBoard(pX, pY + 3);
            var left3 = (int)GetItemAtActiveBoard(pX - 3, pY);
            var right3 = (int)GetItemAtActiveBoard(pX + 3, pY);

            var top4 = (int)GetItemAtActiveBoard(pX, pY - 4);
            var bottom4 = (int)GetItemAtActiveBoard(pX, pY + 4);
            var left4 = (int)GetItemAtActiveBoard(pX - 4, pY);
            var right4 = (int)GetItemAtActiveBoard(pX + 4, pY);

            var top5 = (int)GetItemAtActiveBoard(pX, pY - 4);
            var bottom5 = (int)GetItemAtActiveBoard(pX, pY + 4);
            var left5 = (int)GetItemAtActiveBoard(pX - 4, pY);
            var right5 = (int)GetItemAtActiveBoard(pX + 4, pY);

            var top6 = (int)GetItemAtActiveBoard(pX, pY - 4);
            var bottom6 = (int)GetItemAtActiveBoard(pX, pY + 4);
            var left6 = (int)GetItemAtActiveBoard(pX - 4, pY);
            var right6 = (int)GetItemAtActiveBoard(pX + 4, pY);

            var isThereFood = _foodEaten ? 1 : 0;

            var output = new[] { top, top2, top3, top4, top5, top6, bottom, bottom2, bottom3, bottom4,bottom5, bottom6, left, left2, left3, left4, left5, left6, right, right2, right3, right4,right5, right6, isThereFood };
            return output;
        }

        public void MovePlayer(Direction d)
        {
            _foodEaten = false;

            switch (d)
            {
                case Direction.Up:
                    _playerY--;
                    break;
                case Direction.Down:
                    _playerY++;
                    break;
                case Direction.Left:
                    _playerX--;
                    break;
                case Direction.Right:
                    _playerX++;
                    break;
                default:
                    throw new Exception();
            }

            if(GetItemAtActiveBoard(_playerX.Value,_playerY.Value) == ItemAtPoint.Food)
            {
                _foodEaten = true;
                ClearItemAtActiveBoard(_playerX.Value, _playerY.Value);
            }
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

        public void PrintWithPlayerPosition(int numFoodEaten)
        {
            int x = _playerX.Value;
            int y = _playerY.Value;

            for (int k=0;k<_activeGrid[0].Count()+2;k++)
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
