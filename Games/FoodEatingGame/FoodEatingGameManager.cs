using System;
using System.Threading;
using System.Linq;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Games.FoodEatingGame
{
    class FoodEatingGameManager : IDiscreteGameManager
    {
        public DiscreteIOInfo IOInfo { get; }

        private FoodEatingGameBoardIOAdapter _IOAdapter = new FoodEatingGameBoardIOAdapter();
        public IDiscreteGameIOAdapter IOADapter => _IOAdapter;

        public IDiscreteGameStateProvider StateProvider => new SingleRandomFoodEatingGameStateProvider();

        private int _timerLength;

        public FoodEatingGameManager(int timerLength)
        {
            _timerLength = timerLength;

            IOInfo = new DiscreteIOInfo
            (
                inputInfo: new DiscreteDataPayloadInfo(typeof(ItemAtPoint),4, new string[] { "Top","Bottom","Left","Right","IsThereFood" }),
                outputInfo: new DiscreteDataPayloadInfo(typeof(Direction), 1, new string[] { "Output" })
            );
        }

        public void Demonstrate(IDiscreteDecider p, IDiscreteGameState s)
        {
            PlayGame(p, s, true);
        }

        public int Score(IDiscreteDecider p,IDiscreteGameState s)
        {
            return PlayGame(p, s,false);
        }

        private int PlayGame(IDiscreteDecider p,IDiscreteGameState s,bool isDemonstration)
        {
            var b = (s as FoodEatingGameBoard);
            int numFoodEaten = 0;

            if (isDemonstration)
            {
                PrintCurrentState(b, numFoodEaten);
            }

            for (int i = 0; i < _timerLength; i++)
            {
                var dataAtCurrentPosition = new DiscreteDataPayload(typeof(ItemAtPoint), IOADapter.GetOutput(s).Data.Take(4).ToArray());
                var direction = (Direction)p.Decide(dataAtCurrentPosition).SingleItem;

                if(MoveInDirectionAndCheckIfAteFood(direction,b))
                {
                    numFoodEaten++;
                }

                if(isDemonstration)
                {
                    Console.Clear();
                    PrintCurrentState(b, numFoodEaten);
                    Console.WriteLine("Time remaining: " + (_timerLength - i));
                    Thread.Sleep(200);
                }
            }

            return numFoodEaten;
        }

        public bool MoveInDirectionAndCheckIfAteFood(Direction d,FoodEatingGameBoard b)
        {
            IOADapter.SendInput(b, new DiscreteDataPayload(typeof(Direction), (int)d));
            var wasThereFood = IOADapter.GetOutput(b).Data[4] == 1;

            return wasThereFood;
        }

        public void PrintCurrentState(FoodEatingGameBoard b,int foodEaten)
        {
            b.PrintWithPlayerPosition(foodEaten);
        }

    }
}
