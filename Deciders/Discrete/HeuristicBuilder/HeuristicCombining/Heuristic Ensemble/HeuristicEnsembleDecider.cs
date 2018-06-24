using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.HeuristicBuilder;

namespace SimpleGame.Deciders.Discrete.HeuristicBuilder.Heuristic_Ensemble
{
    public class HeuristicEnsembleDecider : IDiscreteDecider
    {
        public DiscreteIOInfo IOInfo => Deciders.First().IOInfo;

        public int NumGenes => Deciders.Sum(d=>d.NumGenes);

        public int TotalComplexity => Deciders.Sum(d => d.TotalComplexity);

        public List<HeuristicBuildingDecider> Deciders;

        public HeuristicEnsembleDecider(List<HeuristicBuildingDecider> deciders)
        {
            Deciders = deciders;
        }

        public IDiscreteDataPayload Decide(IDiscreteDataPayload input)
        {
            Dictionary<int,int> decisionCounts = new Dictionary<int,int>();
            List<Heuristic> heuristics = new List<Heuristic>();

            foreach (var d in Deciders)
            {
                var decision = d.GetHeuristicFromListFor(input);

                if(decision!=null)
                {
                    heuristics.Add(decision);

                    if (!decisionCounts.ContainsKey(decision.ExpectedOutput))
                    {
                        decisionCounts[decision.ExpectedOutput] = 1;
                    }
                    else
                    {
                        decisionCounts[decision.ExpectedOutput]++;
                    }
                }
            }

            if(!decisionCounts.Any())
            {
                return IOInfo.OutputInfo.GetDefualtInstance();
            }


            //var bySpecifity = heuristics.OrderBy(h => h.Conditions.Where(c => c != -1).Count()).ToList();
            //return new DiscreteDataPayload(bySpecifity.First().ExpectedOutput);


            var ordered = decisionCounts.OrderBy(kvp => kvp.Value);
            var output = new DiscreteDataPayload(ordered.Last().Key);

            return output;
        }

        #region notImplemented

        public IDiscreteDecider CrossMutate(IDiscreteDecider decider2, double mutationRate, Random r)
        {
            throw new NotImplementedException();
        }

        public string GetFullStringRepresentation()
        {
            throw new NotImplementedException();
        }

        public IDiscreteDecider GetMutated(double mutationRate, Random r)
        {
            throw new NotImplementedException();
        }

        public void PostGenerationProcessing()
        {
        }

        #endregion
    }
}
