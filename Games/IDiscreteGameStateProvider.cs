namespace SimpleGame.Games
{
    public interface IDiscreteGameStateProvider
    {
        IDiscreteGameState GetStateForNextGeneration();
    }
}
