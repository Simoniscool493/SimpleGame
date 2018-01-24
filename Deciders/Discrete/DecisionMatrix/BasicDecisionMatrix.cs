using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Permutation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using SimpleGame.AI.GeneticAlgorithm;
using System.Text;
using SimpleGame.Deciders.Discrete;
using SimpleGame.AI;
using SimpleGame.Deciders.Discrete.DecisionMatrix;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class BasicDecisionMatrix : IDecisionMatrix
    {
        private static Random r = new Random();

        private Dictionary<IDiscreteDataPayload, IDiscreteDataPayload> _theMatrix;

        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => _theMatrix.Count();
        public int TotalComplexity => _theMatrix.Count() * _theMatrix.First().Key.Data.Length * _theMatrix.First().Value.Data.Length;

        public BasicDecisionMatrix(Dictionary<IDiscreteDataPayload, IDiscreteDataPayload> matrix, DiscreteIOInfo ioInfo)
        {
            _theMatrix = matrix;
            IOInfo = IOInfo;
        }

        public IDiscreteDataPayload Decide(IDiscreteDataPayload input)
        {
            return _theMatrix[input];
        }

        public Dictionary<IDiscreteDataPayload, IDiscreteDataPayload>.KeyCollection GetKeys()
        {
            return _theMatrix.Keys;
        }

        public bool ContainsKey(IDiscreteDataPayload d)
        {
            return _theMatrix.ContainsKey(d);
        }

        public IDiscreteDecider CrossMutate(IDiscreteDecider decider2, double mutationRate, Random r)
        {
            return DecisionMatrixFactory.MatrixCrossMutate(this,(IDecisionMatrix)decider2, mutationRate, r);
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

        public void PostGenerationProcessing() { }
    }
}
