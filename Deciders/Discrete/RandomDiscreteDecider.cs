using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleGame.Deciders.Discrete
{
    class RandomDiscreteDecider : IDiscreteDecider
    {
        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => 1;
        private Random _r;

        public RandomDiscreteDecider(Random r, DiscreteIOInfo ioInfo)
        {
            IOInfo = ioInfo;
            _r = r;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return IOInfo.OutputInfo.GetRandomInstance(_r);
        }

        public GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species2, double mutationRate, Random r)
        {
            throw new Exception("Crossing Random Matrixes has no effect");
        }

        public void PostGenerationProcessing() { }

        public string GetRaw()
        {
            return "Random Decider";
        }
    }
}
