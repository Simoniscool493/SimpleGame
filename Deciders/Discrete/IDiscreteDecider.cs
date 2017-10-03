using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Deciders.Discrete
{
    public interface IDiscreteDecider
    {
        DiscreteIOInfo IOInfo { get; }

        DiscreteDataPayload Decide(DiscreteDataPayload input);
    }
}
