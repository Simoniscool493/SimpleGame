using SimpleGame.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.DataPayloads.DiscreteData.DecisionMatrix
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
