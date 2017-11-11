using SimpleGame.AI;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;
using SimpleGame.Games.SimplePacman;
using SimpleGame.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Deciders.HeuristicBuilder
{
    class SinglePathMutationRunner
    {
        public DeciderSpecies CurrentBest;
        public HeuristicBuildingDecider BestDecider => ((HeuristicBuildingDecider)CurrentBest.BaseDecider);

        //public double MutationRate;
        public int GenerationSize;
        public int TimesToTestPerSpecies;

        public int MaxConditionsToTake;
        public int MaxHeuristicsToTake;

        private IDiscreteGameManager _game;
        private IDiscreteGameStateProvider _provider;

        public SinglePathMutationRunner(IDiscreteGameManager game, IDiscreteGameStateProvider provider)
        {
            _game = game;
            _provider = provider;
        }

        public void Optimize(int numIterations,Random r)
        {
            for(int i=0;i<numIterations;i++)
            {
                Console.WriteLine("\nMutating Heuristics\n");
                LookForBetterScore(50,r);
                Console.WriteLine("\nRemoving Heuristics\n");
                Simplify(30,r);
                Console.WriteLine("\nRemoving Conditions\n");
                RemoveConditions(30,r);

                BestDecider.PostGenerationProcessing();
                //Console.ReadLine();


            }

        }

        private void IterateChange(Func<DeciderSpecies> processSpecies, int numIterations)
        {
            var deciders = new List<DeciderSpecies>();

            for (int i = 0; i < numIterations; i++)
            {
                deciders.Clear();

                while (deciders.Count() < GenerationSize)
                {
                    DeciderSpecies decider = processSpecies();
                    var heuristicAvg = SimpleGameTester.SuccessTesting(decider, TimesToTestPerSpecies);
                    decider.Score = (int)heuristicAvg;
                    deciders.Add(decider);
                }

                var oldBest = CurrentBest;


                CurrentBest = deciders.OrderBy(d => d.Score).Reverse().First();

                if (CurrentBest.Score < oldBest.Score)
                {
                    CurrentBest = oldBest;
                }

                Console.WriteLine("Score: " + CurrentBest.Score + " Genes: " + CurrentBest.NumGenes + " Conditions: " + BestDecider.Heuristics.Select(h => h.Conditions.Count).Sum());// + " Avg: " + deciders.Select(d=>d.Score).Average().ToString("0.0"));
            }
        }


        private void LookForBetterScore(int numIterations, Random r)
        {
            var mutationRate = (BestDecider.NumGenes == 0) ? 0.05 : (5.0 / BestDecider.NumGenes);

            IterateChange((() =>
            {
                var mutated = (DeciderSpecies)CurrentBest.GetMutated(mutationRate, r);
                return mutated;
            }),
            numIterations);

        }

        private void Simplify(int numIterations, Random r)
        {
            IterateChange((() => 
            {
                var toTake = r.Next(1, MaxHeuristicsToTake);
                var less = ((HeuristicBuildingDecider)CurrentBest.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomHeuristics(toTake);
                return new DeciderSpecies(less);
            }), 
            numIterations);
        }

        private void RemoveConditions(int numIterations,Random r)
        {
            IterateChange((() =>
            {
                var toTake = ((double)r.Next(1, MaxConditionsToTake))/ (BestDecider.NumGenes);

                var less = ((HeuristicBuildingDecider)CurrentBest.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomConditions(toTake,r);
                return new DeciderSpecies(less);
            }),
            numIterations);
        }

    }
}
