using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class MatrixDecider : IDiscreteDecider
    {
        protected DecisionMatrix matrix;

        public MatrixDecider(DecisionMatrix d)
        {
            matrix = d;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return matrix.Decide(input);
        }
    }
}
