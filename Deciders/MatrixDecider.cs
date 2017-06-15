using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders
{
    class MatrixDecider : IDecider
    {
        private DecisionMatrix matrix;

        public MatrixDecider(DecisionMatrix d)
        {
            matrix = d;
        }

        public Direction GetDirection(ItemAtPoint[] upDownLeftright)
        {
            return matrix.Decide(upDownLeftright);
        }
    }
}
