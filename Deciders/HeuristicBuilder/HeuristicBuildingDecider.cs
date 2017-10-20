using SimpleGame.Deciders.Discrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.HeuristicBuilder;

namespace SimpleGame.Deciders
{
    public class HeuristicBuildingDecider : IDiscreteDecider
    {
        public DiscreteIOInfo IOInfo { get; }
        public int NumGenes => Heuristics.Count;

        public List<Heuristic> Heuristics;

        private Random _r;

        public HeuristicBuildingDecider(Random r,DiscreteIOInfo ioInfo)
        {
            Heuristics = new List<Heuristic>();
            IOInfo = ioInfo;
            _r = r;
        }

        public GeneticAlgorithmSpecies Cross(GeneticAlgorithmSpecies species2, double mutationRate, Random r)
        {
            throw new NotImplementedException();
        }

        public DiscreteDataPayload Decide(DiscreteDataPayload input)
        {
            DiscreteDataPayload decision = DecideBasedOnHeuristics(input);

            if(decision == null)
            {
                return IOInfo.OutputInfo.GetRandomInstance(_r);
            }

            return decision;
        }

        public DiscreteDataPayload DecideBasedOnHeuristics(DiscreteDataPayload input)
        {
            foreach(var h in Heuristics)
            {
                if(input.Data[h.PositionInPayload] == h.ExpectedInput)
                {
                    return new DiscreteDataPayload(IOInfo.OutputInfo.PayloadType, h.ExpectedOutput);
                }
            }

            return null;
        }

        public void AddRandomHeuristics(int numToAdd)
        {
            for(int i=0;i<numToAdd;i++)
            {
                var newHeuristic = Heuristic.CreateRandom(_r, IOInfo);
                Heuristics.Add(newHeuristic);
            }
        }

        public void SaveToFile(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
