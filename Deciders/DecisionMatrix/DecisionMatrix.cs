using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Permutation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class DecisionMatrix : IDecisionMatrix
    {
        private static Random r = new Random();

        private Dictionary<DiscreteDataPayload, DiscreteDataPayload> _theMatrix;

        public DiscreteIOInfo IOInfo { get; }

        public DecisionMatrix(Dictionary<DiscreteDataPayload, DiscreteDataPayload> matrix, DiscreteIOInfo ioInfo)
        {
            _theMatrix = matrix;
            IOInfo = IOInfo;

        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return _theMatrix[input];
        }

        public Dictionary<DiscreteDataPayload, DiscreteDataPayload>.KeyCollection GetKeys()
        {
            return _theMatrix.Keys;
        }

        public bool ContainsKey(DiscreteDataPayload d)
        {
            return _theMatrix.ContainsKey(d);
        }

        public static IDecisionMatrix GetRandomIOMapping(Random r, DiscreteIOInfo IOInfo)
        {
            var permutator = new DiscreteDataPayloadPermutator(IOInfo.InputInfo);
            var matrix = new Dictionary<DiscreteDataPayload, DiscreteDataPayload>();
            var isRunning = true;

            while (isRunning)
            {
                var input = permutator.GetAsEnum(IOInfo.InputInfo.PayloadType);
                var randomOutput = IOInfo.OutputInfo.GetRandomInstance(r);
                matrix[input] = randomOutput;

                isRunning = permutator.TryIncrement(0);
            }

            return new DecisionMatrix(matrix, IOInfo);
        }

        public static IDecisionMatrix GetLazyIOMapping(Random r, DiscreteIOInfo IOInfo)
        {
            var matrix = new Dictionary<DiscreteDataPayload, DiscreteDataPayload>();
            return new LazyDecisionMatrix(matrix, IOInfo);
        }

        public void SaveToFile(string fileName)
        {
            Stream saver = File.OpenWrite(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saver, this);
            saver.Close();
        }
    }
}
