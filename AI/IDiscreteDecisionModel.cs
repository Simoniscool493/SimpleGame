using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;

namespace SimpleGame.AI
{
    interface IDiscreteDecisionModel
    {
        IDiscreteDecider Train(IDiscreteGame g,IGameStateProvider provider, bool showProgress);
    }
}
