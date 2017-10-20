using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Permutation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using SimpleGame.AI.GeneticAlgorithm;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class DecisionMatrix : IDecisionMatrix
    {
        private static Random r = new Random();

        private Dictionary<DiscreteDataPayload, DiscreteDataPayload> _theMatrix;

        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => _theMatrix.Count();

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

        public GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species2, double mutationRate, Random r)
        {
            return MatrixCross(this,(IDecisionMatrix)(species2.BaseDecider), mutationRate, r);
        }

        public static GeneticAlgorithmSpecies MatrixCross(IDecisionMatrix matrix1, IDecisionMatrix matrix2, double mutationRate, Random r)
        {
            if (!((matrix1.IOInfo.InputInfo.PayloadType == matrix2.IOInfo.InputInfo.PayloadType) && matrix1.IOInfo.OutputInfo.PayloadType == matrix2.IOInfo.OutputInfo.PayloadType))
            {
                throw new Exception("Cannot cross matrixes of two different payload types");
            }

            var outputValues = matrix1.IOInfo.OutputInfo.PayloadType.GetEnumValues();
            var childMatrix = new Dictionary<DiscreteDataPayload, DiscreteDataPayload>();

            foreach (var key in matrix1.GetKeys())
            {
                if (r.NextDouble() < mutationRate)
                {
                    //if(r.NextDouble()>0.2) // Experiment: remove a gene instead of mutate it
                    //{
                        var value = outputValues.GetValue(r.Next(0, outputValues.Length));
                        var valueAsIntArray = new int[] { ((int)value) };
                        childMatrix[key] = new DiscreteDataPayload(matrix1.IOInfo.OutputInfo.PayloadType, valueAsIntArray);
                    //}
                }
                else if (r.NextDouble() > 0.5)
                {
                    childMatrix[key] = matrix1.Decide(key);
                }
                else
                {
                    childMatrix[key] = matrix2.Decide(key);
                }
            }

            if (matrix1 is LazyDecisionMatrix)
            {
                foreach (var key in matrix2.GetKeys())
                {
                    if (!matrix1.ContainsKey(key))
                    {
                        childMatrix[key] = matrix2.Decide(key);
                    }
                }

                return new GeneticAlgorithmSpecies(new LazyDecisionMatrix(childMatrix, matrix1.IOInfo));
            }

            return new GeneticAlgorithmSpecies(new DecisionMatrix(childMatrix, matrix1.IOInfo));
        }
    }
}
