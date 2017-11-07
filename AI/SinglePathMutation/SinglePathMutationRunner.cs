using SimpleGame.AI;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;
using SimpleGame.Games.SimplePacman;
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

        public double MutationRate;
        public int GenerationSize;
        public int TimesToTestPerSpecies;

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
                Simplify(30);
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
                    var heuristicAvg = Program.SuccessTesting(decider, TimesToTestPerSpecies);
                    decider.Score = (int)heuristicAvg;
                    deciders.Add(decider);
                }

                var oldBest = CurrentBest;


                CurrentBest = deciders.OrderBy(d => d.Score).Reverse().First();

                if (CurrentBest.Score < oldBest.Score)
                {
                    CurrentBest = oldBest;
                }

                Console.WriteLine("Score: " + CurrentBest.Score + " Genes: " + CurrentBest.NumGenes + " Conditions: " + BestDecider.Heuristics.Select(h=>h.Conditions.Count).Sum());
            }
        }


        private void LookForBetterScore(int numIterations, Random r)
        {
            IterateChange((() => (DeciderSpecies)CurrentBest.GetMutated(MutationRate, r)), numIterations);
        }

        private void Simplify(int numIterations)
        {
            IterateChange((() => 
            {
                var less = ((HeuristicBuildingDecider)CurrentBest.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomHeuristics(5);
                return new DeciderSpecies(less);
            }), 
            numIterations);
        }

        private void RemoveConditions(int numIterations,Random r)
        {
            var rate = 1.0 / (BestDecider.NumGenes);
            IterateChange((() =>
            {
                var less = ((HeuristicBuildingDecider)CurrentBest.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomConditions(rate,r);
                return new DeciderSpecies(less);
            }),
            numIterations);
        }

    }
}
