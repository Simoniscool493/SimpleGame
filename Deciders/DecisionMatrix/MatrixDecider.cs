using System;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using SimpleGame.AI.GeneticAlgorithm;

namespace SimpleGame.Deciders.DecisionMatrix
{
    class MatrixDecider : IDiscreteDecider
    {
        protected IDecisionMatrix matrix;
        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => matrix.NumGenes;

        public MatrixDecider(IDecisionMatrix d,DiscreteIOInfo ioInfo)
        {
            matrix = d;
            IOInfo = ioInfo;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return matrix.Decide(input);
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
            return matrix.Cross(species2, mutationRate, r);
        }

        public void PostGenerationProcessing() { }

        public string GetRaw()
        {
            return matrix.GetRaw();
        }


    }
}
