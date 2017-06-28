using SimpleGame.DataPayloads;
using SimpleGame.Games;
using System;
using System.Threading;
using System.Linq;
using SimpleGame.Games.FoodEatingGame;

namespace SimpleGame
{
    class FoodEatingGameRunner : IDiscreteGame
    {
        private FoodEatingGameBoard b;
        private bool _shouldIPause;
        private bool _shouldIPrint;

        private int _currentX;
        private int _currentY;
        private int _timerLength;

        public FoodEatingGameRunner(FoodEatingGameBoard b,bool shouldIPause,bool shouldIPrint,int timerLength)
        {
            this.b = b;
            _shouldIPause = shouldIPause;
            _shouldIPrint = shouldIPrint;
            _timerLength = timerLength;
        }

        public int Score(IDiscreteDecider p,IGameState s)
        {
            var b = (s as FoodEatingGameBoard);

            _currentX = 0;
            _currentY = 0;

            int numFoodEaten = 0;

            if (_shouldIPrint)
            {
                PrintCurrentState(b, numFoodEaten);
            }

            for (int i = 0; i < _timerLength; i++)
            {
                var dataAtCurrentPosition = new DiscreteDataPayload(typeof(ItemAtPoint),GetDataAtCurrentPosition(b).Select(e=>(int)e).ToArray());
                var direction = (Direction)p.Decide(dataAtCurrentPosition).SingleItem;

                if(MoveInDirectionAndCheckIfAteFood(direction,b))
                {
                    numFoodEaten++;
                }

                if(_shouldIPrint)
                {
                    Console.Clear();
                    PrintCurrentState(b, numFoodEaten);
                    Thread.Sleep(50);
                }

                if (_shouldIPause)
                {
                    Console.ReadLine();
                }
            }

            if (_shouldIPrint)
            {
                Console.WriteLine("Done");
            }


            return numFoodEaten;
        }

        public bool MoveInDirectionAndCheckIfAteFood(Direction d,FoodEatingGameBoard b)
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

        public void PrintCurrentState(FoodEatingGameBoard b,int foodEaten)
        {
            b.PrintWithPlayerPosition(_currentX, _currentY,foodEaten);
        }

        public ItemAtPoint[] GetDataAtCurrentPosition(FoodEatingGameBoard b)
        {
            var top = b.GetItemAtActiveBoard(_currentX, _currentY-1);
            var bottom = b.GetItemAtActiveBoard(_currentX, _currentY+1);
            var left = b.GetItemAtActiveBoard(_currentX-1, _currentY);
            var right = b.GetItemAtActiveBoard(_currentX+1, _currentY);

            return new[] { top, bottom, left, right };
        }

        static FoodEatingGameBoard randomBoard = FoodEatingGameBoard.GetRandomBoard();

        public IGameState GetState()
        {
            randomBoard.Reset();
            return randomBoard;
        }
    }
}
