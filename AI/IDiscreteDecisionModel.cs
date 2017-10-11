using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;

namespace SimpleGame.AI
{
    interface IDiscreteDecisionModel
    {
        IDiscreteDecider Train(IDiscreteGameManager g,IDiscreteGameStateProvider provider, bool showGameProgress, bool printBasicInfo, int demonstrateEveryXIterations);
    }
}
