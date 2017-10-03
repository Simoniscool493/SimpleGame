using System;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class MatrixDecider : IDiscreteDecider
    {
        protected IDecisionMatrix matrix;
        public DiscreteIOInfo IOInfo { get; }

        public MatrixDecider(IDecisionMatrix d,DiscreteIOInfo ioInfo)
        {
            matrix = d;
            IOInfo = ioInfo;
        }


        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return matrix.Decide(input);
        }
    }
}
