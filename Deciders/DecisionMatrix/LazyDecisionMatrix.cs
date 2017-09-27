using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class LazyDecisionMatrix : IDecisionMatrix
    {
        private static Random r = new Random();
        private Dictionary<DiscreteDataPayload, DiscreteDataPayload> _theMatrix;

        public DiscreteIOInfo IOInfo;
        public Type InputType { get; }
        public Type OutputType { get; }


        public LazyDecisionMatrix(Dictionary<DiscreteDataPayload, DiscreteDataPayload> matrix,DiscreteIOInfo ioInfo)
        {
            _theMatrix = matrix;
            IOInfo = ioInfo;
            InputType = matrix.Keys.First().UnderlyingType;
            OutputType = matrix.Values.First().UnderlyingType;

        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            if(_theMatrix.ContainsKey(input))
            {
                return (_theMatrix[input]);
            }
            else
            {
                var instance = IOInfo.OutputInfo.GetRandomInstance(r);
                _theMatrix[input] = instance;
                return instance;
            }
        }

        public IEnumerable<DiscreteDataPayload> GetKeys()
        {
            throw new NotImplementedException();
        }
    }
}
