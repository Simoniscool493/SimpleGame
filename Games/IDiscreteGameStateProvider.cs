namespace SimpleGame.Games
{
    public interface IDiscreteGameStateProvider
    {
        IDiscreteGameState GetStateForNextGeneration();

        IDiscreteGameState GetStateForDemonstration();
    }
}
