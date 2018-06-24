using SimpleGame.Games.SimplePacman;
using SimpleGame.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.Discrete.HeuristicBuilder.HeuristicCombining
{
    class HeuristicCombiner
    {
        public static HeuristicBuildingDecider ExpandAndCombine(List<HeuristicBuildingDecider> list,PacmanManager manager)
        {
            var r = new Random();

            /*var expandedList = new List<HeuristicBuildingDecider>();
            var finalDecider = new HeuristicBuildingDecider(new Random(), manager.IOInfo, list.First().NumConditionsToBuildFrom, null);

            foreach (var decider in list)
            {
                if(!decider.RandomSeedRange.IsSingle)
                {
                    throw new Exception();

                }
                var expandedDecider = (HeuristicBuildingDecider)manager.RunWhileSavingAllDecisions(decider, manager.StateProvider.GetStateForTraining(decider.RandomSeedRange.RangeStartInclusive));
                finalDecider.Heuristics.AddRange(expandedDecider.Heuristics);
            }

            var scoreFinal = (int)SimpleGameTester.SetRandomSuccessTesting(manager, finalDecider, list.Count, 0);*/


            ///


            var deciderRand0 = list[0];
            var deciderRand1 = list[1];

            var score0 = (int)SimpleGameTester.SetRandomSuccessTesting(manager, deciderRand0, 1, 0);
            var score1 = (int)SimpleGameTester.SetRandomSuccessTesting(manager, deciderRand1, 1, 1);

            var deciderRand0Expanded = (HeuristicBuildingDecider)manager.RunWhileSavingAllDecisions(deciderRand0, manager.StateProvider.GetStateForTraining(0));
            var deciderRand1Expanded = (HeuristicBuildingDecider)manager.RunWhileSavingAllDecisions(deciderRand1, manager.StateProvider.GetStateForTraining(1));


            var score0expanded = (int)SimpleGameTester.SetRandomSuccessTesting(manager, deciderRand0Expanded, 1, 0);
            var score1expanded = (int)SimpleGameTester.SetRandomSuccessTesting(manager, deciderRand1Expanded, 1, 1);

    

            Console.WriteLine("Done step one");
            int bestScore=  0;
            List<int> scores = new List<int>();
            while(bestScore<240)
            {
                var deciderWithBoth = new HeuristicBuildingDecider(new Random(r.Next()), manager.IOInfo, ((HeuristicBuildingDecider)deciderRand0Expanded).NumConditionsToBuildFrom, null);
                deciderWithBoth.Heuristics.AddRange(deciderRand0Expanded.Heuristics);
                deciderWithBoth.Heuristics.AddRange(deciderRand1Expanded.Heuristics);

                var score0combined = (int)SimpleGameTester.SetRandomSuccessTesting(manager, deciderWithBoth, 1, 0);
                var score1combined = (int)SimpleGameTester.SetRandomSuccessTesting(manager, deciderWithBoth, 1, 1);

                if(score1combined>bestScore)
                {
                    bestScore = score1combined;
                    Console.WriteLine(score0combined + ", " + score1combined);
                }

                scores.Add(score1combined);
            }


            ///
            return null;
        }
    }
}
