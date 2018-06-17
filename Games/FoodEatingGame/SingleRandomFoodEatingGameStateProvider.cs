using System;

namespace SimpleGame.Games.FoodEatingGame
{
    class SingleRandomFoodEatingGameStateProvider : IDiscreteGameStateProvider
    {
        private FoodEatingGameBoard _singleRandomBoard;

        public IDiscreteGameState GetStateForDemonstration(int randomSeed)
        {
            _singleRandomBoard = FoodEatingGameBoard.GetRandomBoard(randomSeed);
            return _singleRandomBoard;
        }

        public IDiscreteGameState GetStateForTraining(int randomSeed)
        {
            _singleRandomBoard = FoodEatingGameBoard.GetRandomBoard(randomSeed);
            return _singleRandomBoard;
        }
    }
}
