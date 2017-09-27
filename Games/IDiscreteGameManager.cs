using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Games
{
    public interface IDiscreteGameManager
    {
        DiscreteIOInfo IOInfo { get; }

        IDiscreteGameIOAdapter IOADapter { get; }

        IDiscreteGameStateProvider StateProvider { get; }

        int Score(IDiscreteDecider decider,IDiscreteGameState state);

        void Demonstrate(IDiscreteDecider decider, IDiscreteGameState state);
    }
}
