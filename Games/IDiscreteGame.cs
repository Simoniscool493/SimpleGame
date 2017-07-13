using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Games
{
    public interface IDiscreteGame
    {
        DiscreteIOInfo IOInfo { get; }

        int Score(IDiscreteDecider decider,IGameState state);

        void Demonstrate(IDiscreteDecider decider, IGameState state);

        IGameState GetNextStateForTraining();
    }
}
