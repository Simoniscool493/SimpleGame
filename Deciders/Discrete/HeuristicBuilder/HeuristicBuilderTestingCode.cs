using SimpleGame.Games.SimplePacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.HeuristicBuilder
{
    class HeuristicBuilderTestingCode
    {
        public static void Run()
        {
            var runner = new PacmanManager();

           var heuristicDecidersByScore = new Dictionary<HeuristicBuildingDecider, double>();
            var r = new Random();

            for (int i = 0; i < 1000; i++)
            {
                var heuristicDecider = new HeuristicBuildingDecider(r, runner.IOInfo);
                heuristicDecider.AddRandomHeuristics(5);
                var heuristicAvg = Program.SuccessTesting(heuristicDecider, 1);

                heuristicDecidersByScore[heuristicDecider] = heuristicAvg;
            }

            var best = heuristicDecidersByScore.OrderBy(kv => kv.Value).Reverse().First();

            for (int i = 0; i < 100000; i++)
            {
                heuristicDecidersByScore.Clear();

                for (int j = 0; j < 20; j++)
                {
                    var heuristicDecider = best.Key.GetMutated(r.Next(2, 4));
                    var heuristicAvg = Program.SuccessTesting(heuristicDecider, 1);
                    heuristicDecidersByScore[heuristicDecider] = heuristicAvg;
                }

                var oldBest = new KeyValuePair<HeuristicBuildingDecider, double>(best.Key, Program.SuccessTesting(best.Key, 1));


                best = heuristicDecidersByScore.OrderBy(kv => kv.Value).Reverse().First();

                if (best.Value < oldBest.Value)
                {
                    best = oldBest;
                }

                Console.WriteLine("Score: " + best.Value + " Genes: " + best.Key.NumGenes);
            }

            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(best.Key, state);

        }
    }
}
