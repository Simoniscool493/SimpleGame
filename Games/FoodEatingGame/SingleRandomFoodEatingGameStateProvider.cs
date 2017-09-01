namespace SimpleGame.Games.FoodEatingGame
{
    class SingleRandomFoodEatingGameStateProvider : IGameStateProvider
    {
        private FoodEatingGameBoard _singleRandomBoard = FoodEatingGameBoard.GetRandomBoard();

        public IGameState GetStateForNextGeneration()
        {
            _singleRandomBoard.Reset();
            return _singleRandomBoard;
        }
    }
}
