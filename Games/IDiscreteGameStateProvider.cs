namespace SimpleGame.Games
{
    public interface IDiscreteGameStateProvider
    {
        int RandomSeed { get; set; }

        IDiscreteGameState GetStateForNextGeneration();

        IDiscreteGameState GetStateForDemonstration();
    }
}
