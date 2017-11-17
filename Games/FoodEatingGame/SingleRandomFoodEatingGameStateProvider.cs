using System;

namespace SimpleGame.Games.FoodEatingGame
{
    class SingleRandomFoodEatingGameStateProvider : IDiscreteGameStateProvider
    {
        public int RandomSeed
        {
            get
            {
                throw new Exception();
            }
            set
            {
                throw new Exception();
            }
        }

        private FoodEatingGameBoard _singleRandomBoard = FoodEatingGameBoard.GetRandomBoard();

        public IDiscreteGameState GetStateForDemonstration()
        {
            _singleRandomBoard.Reset();
            return _singleRandomBoard;
        }

        public IDiscreteGameState GetStateForNextGeneration()
        {
            _singleRandomBoard.Reset();
            return _singleRandomBoard;
        }
    }
}
