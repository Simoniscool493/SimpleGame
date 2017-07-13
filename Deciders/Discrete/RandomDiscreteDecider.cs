using SimpleGame.DataPayloads.DiscreteData;
using System;

namespace SimpleGame.Deciders.Discrete
{
    class RandomDiscreteDecider : IDiscreteDecider
    {
        public DiscreteDataPayloadInfo OutputType;
        private Random _r;

        public RandomDiscreteDecider(Random r,DiscreteDataPayloadInfo outputType)
        {
            OutputType = outputType;
            _r = r;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return OutputType.GetRandomInstance(_r);
        }
    }
}
