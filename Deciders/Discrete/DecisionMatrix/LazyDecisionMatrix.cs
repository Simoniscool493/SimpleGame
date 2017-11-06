using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleGame.AI.GeneticAlgorithm;

namespace SimpleGame.Deciders.DecisionMatrix
{
    [Serializable()]
    class LazyDecisionMatrix : IDecisionMatrix
    {
        private static Random r = new Random();
        private Dictionary<DiscreteDataPayload, DiscreteDataPayload> _theMatrix;

        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => _theMatrix.Count();

        public LazyDecisionMatrix(Dictionary<DiscreteDataPayload, DiscreteDataPayload> matrix,DiscreteIOInfo ioInfo)
        {
            _theMatrix = matrix;
            IOInfo = ioInfo;
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

        public Dictionary<DiscreteDataPayload, DiscreteDataPayload>.KeyCollection GetKeys()
        {
            return _theMatrix.Keys;
        }

        public bool ContainsKey(DiscreteDataPayload d)
        {
            return _theMatrix.ContainsKey(d);
        }

        public GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species2, double mutationRate, Random r)
        {
            return BasicDecisionMatrix.MatrixCross(this, (IDecisionMatrix)(species2.BaseDecider), mutationRate, r);
        }

        public void PostGenerationProcessing() { }

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
