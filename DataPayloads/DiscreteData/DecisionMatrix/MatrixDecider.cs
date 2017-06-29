using SimpleGame.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads;

namespace SimpleGame.Deciders
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
