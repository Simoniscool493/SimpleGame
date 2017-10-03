using SimpleGame.DataPayloads.DiscreteData;
using System;

namespace SimpleGame.Deciders.Discrete
{
    class RandomDiscreteDecider : IDiscreteDecider
    {
        public DiscreteIOInfo IOInfo { get; }
        private Random _r;

        public RandomDiscreteDecider(Random r,DiscreteIOInfo ioInfo)
        {
            IOInfo = ioInfo;
            _r = r;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return IOInfo.OutputInfo.GetRandomInstance(_r);
        }
    }
}
