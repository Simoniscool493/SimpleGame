using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Deciders.Discrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleGame.AI.GeneticAlgorithm
{
    [Serializable()]
    public class GeneticAlgorithmSpecies : IDiscreteDecider
    {
        public bool IsScored = false;
        public IDiscreteDecider BaseDecider;
        public int Score;

        public DiscreteIOInfo IOInfo => BaseDecider.IOInfo;
        public int NumGenes => BaseDecider.NumGenes;

        public GeneticAlgorithmSpecies(IDiscreteDecider matrix)
        {
            BaseDecider = matrix;
        }

        public override string ToString()
        {
            return "Score: " + Score.ToString() + " Genes: " + NumGenes.ToString();
        }

        public GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species2,double mutationRate,Random r)
        {
            return BaseDecider.Cross(species2, mutationRate, r);
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return BaseDecider.Decide(input);
        }

        public void SaveToFile(string fileName)
        {
            Stream saver = File.OpenWrite(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saver, this);
            saver.Close();
        }

        public void PostGenerationProcessing() { }
    }
}
