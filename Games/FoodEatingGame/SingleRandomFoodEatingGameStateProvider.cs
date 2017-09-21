namespace SimpleGame.Games.FoodEatingGame
{
    class SingleRandomFoodEatingGameStateProvider : IDiscreteGameStateProvider
    {
        private FoodEatingGameBoard _singleRandomBoard = FoodEatingGameBoard.GetRandomBoard();

        public IDiscreteGameState GetStateForNextGeneration()
        {
            _singleRandomBoard.Reset();
            return _singleRandomBoard;
        }
    }
}
