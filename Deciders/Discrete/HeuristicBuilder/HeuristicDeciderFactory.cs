using SimpleGame.Deciders.HeuristicBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.Discrete.HeuristicBuilder
{
    class HeuristicDeciderFactory
    {
        public static HeuristicBuildingDecider GetMutated(HeuristicBuildingDecider decider,double mutationRate, Random r)
        {
            var mutatedDecider = new HeuristicBuildingDecider(r, decider.IOInfo,decider.NumConditionsToBuildFrom);

            foreach(Heuristic h in decider.Heuristics)
            {
                if(r.NextDouble()<mutationRate)
                {
                    mutatedDecider.Heuristics.Add(h.GetMutated(r));
                }
                else
                {
                    mutatedDecider.Heuristics.Add(h.GetCopy(true));
                }
            }

            return mutatedDecider;
        }


        public static HeuristicBuildingDecider CrossMutate(HeuristicBuildingDecider decider1, HeuristicBuildingDecider decider2, double mutationRate, Random r)
        {
            throw new NotImplementedException();
            /*var heuristics1 = decider1.Heuristics;
            var heuristics2 = decider2.Heuristics;
            var IOInfo = decider1.IOInfo;

            var outputValues = IOInfo.OutputInfo.EnumValues;
            var childDecider = new HeuristicBuildingDecider(r, IOInfo);
            var childHeuristics = childDecider.Heuristics;

            foreach (var h in heuristics1)
            {
                if (r.NextDouble() < mutationRate)
                {
                    var value = outputValues.GetValue(r.Next(0, outputValues.Length));
                    var valueAsIntArray = new int[] { ((int)value) };

                    var mutatedH = new Heuristic((int)value, IOInfo);
                    mutatedH.Conditions = new List<Tuple<int, int>>(h.Conditions);
                    mutatedH.Exceptions = new List<Tuple<int, int>>(h.Exceptions);
                    mutatedH.UseCount = h.UseCount;

                    childHeuristics.Add(mutatedH);
                }
                else if (r.NextDouble() > 0.5)
                {
                    var parentH = decider1.GetExactHeuristicOrRandomForThisInput(h.RecreatePayloadWithConditions());

                    var childH = new Heuristic(parentH.ExpectedOutput, IOInfo);
                    childH.Conditions = new List<Tuple<int, int>>(parentH.Conditions);
                    childH.Exceptions = new List<Tuple<int, int>>(parentH.Exceptions);
                    childH.UseCount = parentH.UseCount;
                    childDecider.Heuristics.Add(childH);
                }
                else
                {
                    var parentH = decider2.GetExactHeuristicOrRandomForThisInput(h.RecreatePayloadWithConditions());

                    var childH = new Heuristic(parentH.ExpectedOutput, IOInfo);
                    childH.Conditions = new List<Tuple<int, int>>(parentH.Conditions);
                    childH.Exceptions = new List<Tuple<int, int>>(parentH.Exceptions);
                    childH.UseCount = parentH.UseCount;
                    childDecider.Heuristics.Add(childH);
                }
            }

            //no code yet exists to add parent 2's heuristics

            return childDecider;*/
        }
    }
}
