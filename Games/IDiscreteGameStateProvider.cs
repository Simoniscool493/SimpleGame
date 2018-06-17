namespace SimpleGame.Games
{
    public interface IDiscreteGameStateProvider
    {
        IDiscreteGameState GetStateForTraining(int randomSeed);

        IDiscreteGameState GetStateForDemonstration(int randomSeed);
    }
}
