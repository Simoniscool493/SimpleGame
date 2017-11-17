using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.AI;
using SimpleGame.Deciders.Discrete.DecisionMatrix;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Deciders.DecisionMatrix
{
    [Serializable()]
    class LazyDecisionMatrix : IDecisionMatrix
    {
        private static Random r = new Random();
        private Dictionary<IDiscreteDataPayload, IDiscreteDataPayload> _theMatrix;

        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => _theMatrix.Count();

        public LazyDecisionMatrix(Dictionary<IDiscreteDataPayload, IDiscreteDataPayload> matrix,DiscreteIOInfo ioInfo)
        {
            _theMatrix = matrix;
            IOInfo = ioInfo;
        }

        public IDiscreteDataPayload Decide(IDiscreteDataPayload input)
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

        public Dictionary<IDiscreteDataPayload, IDiscreteDataPayload>.KeyCollection GetKeys()
        {
            return _theMatrix.Keys;
        }

        public bool ContainsKey(IDiscreteDataPayload d)
        {
            return _theMatrix.ContainsKey(d);
        }

        public IDiscreteDecider CrossMutate(IDiscreteDecider decider, double mutationRate, Random r)
        {
            return DecisionMatrixFactory.MatrixCrossMutate(this, (IDecisionMatrix)(decider), mutationRate, r);
        }

        public IDiscreteDecider GetMutated(double mutationRate, Random r)
        {
            throw new NotImplementedException();
        }

        public string GetRaw()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var i in _theMatrix)
            {
                string data = "";
                foreach (var c in i.Key.Data)
                {
                    data = data + " " + c;
                }
                data = data + '\t';

                data = data + i.Value.SingleItem.ToString();

                sb.AppendLine(data);
            }

            return sb.ToString();
        }

    }
}
