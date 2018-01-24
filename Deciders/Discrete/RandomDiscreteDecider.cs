using SimpleGame.AI;
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
        public int TotalComplexity => 1;

        private Random _r;

        public RandomDiscreteDecider(Random r, DiscreteIOInfo ioInfo)
        {
            IOInfo = ioInfo;
            _r = r;
        }

        public IDiscreteDataPayload Decide(IDiscreteDataPayload input)
        {
            return IOInfo.OutputInfo.GetRandomInstance(_r);
        }

        public IDiscreteDecider CrossMutate(IDiscreteDecider species2, double mutationRate, Random r)
        {
            throw new Exception("Crossing Random Matrixes has no effect");
        }

        public IDiscreteDecider GetMutated(double mutationRate, Random r)
        {
            throw new Exception("Mutating Random Matrixes has no effect");
        }

        public string GetRaw()
        {
            return "Random Decider";
        }

        public void PostGenerationProcessing() { }
    }
}
