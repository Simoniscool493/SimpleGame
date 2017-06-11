using System;

namespace SimpleGame
{
    class GameRunner
    {
        private bool _shouldIPause;
        private bool _shouldIPrint;

        private int _currentX;
        private int _currentY;
        private int _timerLength;

        public GameRunner(bool shouldIPause,bool shouldIPrint,int startX,int startY,int timerLength)
        {
            _shouldIPause = shouldIPause;
            _shouldIPrint = shouldIPrint;
            _currentX = startX;
            _currentY = startY;
            _timerLength = timerLength;
        }

        public int RunPlayerOnBoard(IGridPlayer p,GameBoard b)
        {
            b.ResetBoard();
            int numFoodEaten = 0;
            PrintCurrentState(b, numFoodEaten);

            for (int i = 0; i < _timerLength; i++)
            {
                var dataAtCurrentPosition = GetDataAtCurrentPosition(b);
                var direction = p.GetDirection(dataAtCurrentPosition);

                if(MoveInDirectionAndCheckIfAteFood(direction,b))
                {
                    numFoodEaten++;
                }

                if(_shouldIPrint)
                {
                    Console.Clear();
                    PrintCurrentState(b, numFoodEaten);
                }

                if (_shouldIPause)
                {
                    Console.ReadLine();
                }
            }

            Console.WriteLine("Done");

            return numFoodEaten;
        }

        public bool MoveInDirectionAndCheckIfAteFood(Direction d,GameBoard b)
        {
            var wasThereFood = false;
            var newPosition = b.GetPositionInDirection(d, _currentX, _currentY);

            if(newPosition==ItemAtPoint.OutOfBounds)
            {
                return false;
            }

            MoveInDirection(d);

            if (newPosition == ItemAtPoint.Food)
            {
                b.ClearItemAtActiveBoard(_currentX, _currentY);
                wasThereFood = true;
            }

            return wasThereFood;
        }

        public void MoveInDirection(Direction d)
        {
            switch (d)
            {
                case Direction.Up:
                    _currentY--;
                    break;
                case Direction.Down:
                    _currentY++;
                    break;
                case Direction.Left:
                    _currentX--;
                    break;
                case Direction.Right:
                    _currentX++;
                    break;
                default:
                    throw new Exception();
            }
        }

        public void PrintCurrentState(GameBoard b,int foodEaten)
        {
            b.PrintWithPlayerPosition(_currentX, _currentY,foodEaten);
        }

        public ItemAtPoint[] GetDataAtCurrentPosition(GameBoard b)
        {
            var top = b.GetItemAtActiveBoard(_currentX, _currentY-1);
            var bottom = b.GetItemAtActiveBoard(_currentX, _currentY+1);
            var left = b.GetItemAtActiveBoard(_currentX-1, _currentY);
            var right = b.GetItemAtActiveBoard(_currentX+1, _currentY);

            return new[] { top, bottom, left, right };
        }
    }
}
