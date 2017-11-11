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
        public bool IsScored = false;
        public IDiscreteDecider BaseDecider;
        public int Score;

        public int? RandomSeed;

        public DiscreteIOInfo IOInfo => BaseDecider.IOInfo;
        public int NumGenes => BaseDecider.NumGenes;

        public DeciderSpecies(IDiscreteDecider matrix)
        {
            BaseDecider = matrix;
        }

        public IDiscreteDecider CrossMutate(IDiscreteDecider species2,double mutationRate,Random r)
        {
            return new DeciderSpecies(BaseDecider.CrossMutate(((DeciderSpecies)species2).BaseDecider, mutationRate, r));
        }

        public IDiscreteDecider GetMutated(double mutationRate, Random r)
        {
            var species = new DeciderSpecies(BaseDecider.GetMutated(mutationRate,r));
            return species;
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            return BaseDecider.Decide(input);
        }

        public string GetRaw()
        {
            return $"Species score: {Score}\n" + BaseDecider.GetRaw();
        }

        public override string ToString()
        {
            return "Score: " + Score.ToString() + " Genes: " + NumGenes.ToString();
        }

    }
}
