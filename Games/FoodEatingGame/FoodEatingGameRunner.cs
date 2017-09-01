using System;
using System.Threading;
using System.Linq;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Games.FoodEatingGame
{
    class FoodEatingGameRunner : IDiscreteGame
    {
        public DiscreteIOInfo IOInfo { get; }

        private int _currentX;
        private int _currentY;
        private int _timerLength;

        public FoodEatingGameRunner(int timerLength)
        {
            _timerLength = timerLength;

            IOInfo = new DiscreteIOInfo
            (
                inputInfo: new DiscreteDataPayloadInfo(typeof(ItemAtPoint),4),
                outputInfo: new DiscreteDataPayloadInfo(typeof(Direction), 1)
            );
        }

        public void Demonstrate(IDiscreteDecider p, IGameState s)
        {
            PlayGame(p, s, true);
        }

        public int Score(IDiscreteDecider p,IGameState s)
        {
            return PlayGame(p, s,false);
        }

        private int PlayGame(IDiscreteDecider p,IGameState s,bool isDemonstration)
        {
            var b = (s as FoodEatingGameBoard);

            _currentX = 0;
            _currentY = 0;

            int numFoodEaten = 0;

            if (isDemonstration)
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

                if(isDemonstration)
                {
                    Console.Clear();
                    PrintCurrentState(b, numFoodEaten);
                    Thread.Sleep(200);
                }
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
    }
}
