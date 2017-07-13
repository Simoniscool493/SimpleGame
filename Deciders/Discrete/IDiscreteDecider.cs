using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Deciders.Discrete
{
    public interface IDiscreteDecider
    {
        DiscreteDataPayload Decide(DiscreteDataPayload input);
    }
}
