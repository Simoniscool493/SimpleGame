using System;
using System.Threading;
using System.Linq;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games.FoodEatingGame.Payloads;

namespace SimpleGame.Games.FoodEatingGame
{
    class FoodEatingGameManager : IDiscreteGameManager
    {
        public static int RandomSeed = 1;
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
                inputInfo: new FoodEatingGameInputInfo(),
                outputInfo: new FoodEatingGameOutputInfo()
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
                var dataAtCurrentPosition = new DiscreteDataPayload(IOADapter.GetOutput(s).Data.Take(IOInfo.InputInfo.PayloadLength).ToArray());
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
                    Thread.Sleep(20);
                }
            }

            return numFoodEaten;
        }

        public bool MoveInDirectionAndCheckIfAteFood(Direction d,FoodEatingGameBoard b)
        {
            IOADapter.SendInput(b, new DiscreteDataPayload((int)d));
            var wasThereFood = IOADapter.GetOutput(b).Data[IOInfo.InputInfo.PayloadLength-1] == 1;

            return wasThereFood;
        }

        public void PrintCurrentState(FoodEatingGameBoard b,int foodEaten)
        {
            b.PrintWithPlayerPosition(foodEaten);
        }
    }
}
