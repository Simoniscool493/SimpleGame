namespace SimpleGame.Games
{
    public interface IGameStateProvider
    {
        IGameState GetStateForNextGeneration();
    }
}
