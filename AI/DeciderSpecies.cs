using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Deciders.Discrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SimpleGame.AI
{
    [Serializable()]
    public class DeciderSpecies : IDiscreteDecider
    {
        public IDiscreteDecider BaseDecider;
        public DiscreteIOInfo IOInfo => BaseDecider.IOInfo;
        public int TotalComplexity => BaseDecider.TotalComplexity;
        public int NumGenes => BaseDecider.NumGenes;

        public int TimesTried = 0;
        public bool IsScored = false;
        public int Score;
        //TODO replace with score object

        public DeciderSpecies(IDiscreteDecider baseDecider)
        {
            BaseDecider = baseDecider;
        }

        public IDiscreteDecider GetMutated(double mutationRate, Random r)
        {
            return new DeciderSpecies(BaseDecider.GetMutated(mutationRate, r));
        }

        public IDiscreteDataPayload Decide(IDiscreteDataPayload input)
        {
            return BaseDecider.Decide(input);
        }

        public void PostGenerationProcessing()
        {
            BaseDecider.PostGenerationProcessing();
        }

        public string GetFullStringRepresentation()
        {
            return $"Species score: {Score}\n" + BaseDecider.GetFullStringRepresentation();
        }

        public override string ToString()
        {
            return "Score: " + Score.ToString() + " Genes: " + NumGenes.ToString() + " Complexity: " + TotalComplexity.ToString();
        }

        public IDiscreteDecider CrossMutate(IDiscreteDecider species2, double mutationRate, Random r)
        {
            throw new NotImplementedException();
            //return new DeciderSpecies(BaseDecider.CrossMutate(((DeciderSpecies)species2).BaseDecider, mutationRate, r));
        }
    }
}
