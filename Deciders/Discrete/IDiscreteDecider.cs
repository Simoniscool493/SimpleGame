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

        DiscreteDataPayload Decide(DiscreteDataPayload input);

        string GetRaw();

        int NumGenes { get; }

        IDiscreteDecider CrossMutate(IDiscreteDecider decider2, double mutationRate, Random r);

        IDiscreteDecider GetMutated(double mutationRate, Random r);
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
