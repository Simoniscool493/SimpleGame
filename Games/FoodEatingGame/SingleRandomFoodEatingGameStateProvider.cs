using System;

namespace SimpleGame.Games.FoodEatingGame
{
    class SingleRandomFoodEatingGameStateProvider : IDiscreteGameStateProvider
    {
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
