using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Runtime.Serialization;

namespace SimpleGame.Deciders.Discrete
{
    public interface IDiscreteDecider
    {
        DiscreteIOInfo IOInfo { get; }

        DiscreteDataPayload Decide(DiscreteDataPayload input);

        void SaveToFile(string filename);

        int NumGenes { get; }

        GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species2, double mutationRate, Random r);

        void PostGenerationProcessing();
    }
}
