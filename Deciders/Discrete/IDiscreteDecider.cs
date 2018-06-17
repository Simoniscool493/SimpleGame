using SimpleGame.AI;
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleGame.Deciders.Discrete
{
    public interface IDiscreteDecider
    {
        DiscreteIOInfo IOInfo { get; }
        int NumGenes { get; }
        int TotalComplexity { get; }

        IDiscreteDataPayload Decide(IDiscreteDataPayload input);
        IDiscreteDecider GetMutated(double mutationRate, Random r);
        IDiscreteDecider CrossMutate(IDiscreteDecider decider2, double mutationRate, Random r);

        void PostGenerationProcessing();
        string GetFullStringRepresentation();
    }

    public static class DiscreteDeciderExtensions
    {
        public static void SaveToFile(this IDiscreteDecider decider,string fileName)
        {
            Stream saver = File.OpenWrite(fileName);
            BinaryFormatter serializer = new BinaryFormatter();
            serializer.Serialize(saver, decider);
            saver.Close();
        }
    }
}
