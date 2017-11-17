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

        public bool? MinimizeComplexity;
        public bool? IncludePreviousBestWhenIteratingForwards;

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
                LookForBetterScore(50,r); //50
                Console.WriteLine("\nRemoving Heuristics\n");
                Simplify(40,r); //40 */
                Console.WriteLine("\nRemoving Conditions\n");
                RemoveConditions(40,r); //40 

                //Console.WriteLine("\nAdding Exceptions\n");
                //AddExceptions(400, r); 

                BestDecider.PostGenerationProcessing();

                foreach(var h in BestDecider.Heuristics)
                {
                    h.UseCount = 0;
                }

                //CurrentBest.SaveToFile("C:\\ProjectLogs\\GoodHeuristicSets\\" + CurrentBest.Score + "_EatPellets_GeneralSolution.dc");
                //Console.ReadLine();
            }

        }

        private void IterateChange(Func<DeciderSpecies> processSpecies, Action<DeciderSpecies> postProcess, int numIterations)
        {
            var deciders = new List<DeciderSpecies>();

            for (int i = 0; i < numIterations; i++)
            {
                deciders.Clear();

                while (deciders.Count() < GenerationSize)
                {
                    DeciderSpecies decider = processSpecies();
                    var heuristicAvg = SimpleGameTester.SetRandomSuccessTesting(_game,decider, TimesToTestPerSpecies);
                    decider.Score = (int)heuristicAvg;
                    deciders.Add(decider);
                }

                var oldBest = CurrentBest;


                CurrentBest = deciders.OrderBy(d => d.Score).Reverse().First();

                if (IncludePreviousBestWhenIteratingForwards.Value && CurrentBest.Score < oldBest.Score)
                {
                    oldBest.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game,oldBest, TimesToTestPerSpecies);
                    CurrentBest = oldBest;
                }

                if (MinimizeComplexity.Value)
                {
                    deciders.Add(oldBest);
                    var highestScores = deciders.Where(d => d.Score == CurrentBest.Score);

                    CurrentBest = highestScores.OrderBy(d => ((HeuristicBuildingDecider)(d.BaseDecider)).TotalComplexity).First();
                }

                postProcess?.Invoke(CurrentBest);

                Console.WriteLine("Score: " + CurrentBest.Score + " Genes: " + CurrentBest.NumGenes + " Complexity: " + (BestDecider.TotalComplexity));// + " Avg: " + deciders.Select(d=>d.Score).Average().ToString("0.0"));
                //Console.WriteLine("Score: " + ((CurrentBest.Score * 100.0f) / 244.0f).ToString("0.00") + " Genes: " + CurrentBest.NumGenes + " Complexity: " + (BestDecider.TotalComplexity));// + " Avg: " + deciders.Select(d=>d.Score).Average().ToString("0.0"));
            }
        }


        private void LookForBetterScore(int numIterations, Random r)
        {
            var mutationRate = (BestDecider.NumGenes == 0) ? 0.05 : ((double)r.Next(1,10)/ BestDecider.NumGenes);

            IterateChange((() =>
            {
                var mutated = (DeciderSpecies)CurrentBest.GetMutated(mutationRate, r);
                return mutated;
            }),
            null,
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
            null, 
            numIterations);
        }

        private void RemoveConditions(int numIterations,Random r)
        {
            IterateChange((() =>
            {
                var toTake = r.Next(1, MaxConditionsToTake);

                var less = ((HeuristicBuildingDecider)CurrentBest.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomConditions(toTake,r);
                return new DeciderSpecies(less);
            }),
            null,
            numIterations);
        }

        private void AddExceptions(int numIterations, Random r)
        {
            IterateChange(
            (() =>
            {
                var toAddExceptions = ((HeuristicBuildingDecider)CurrentBest.BaseDecider).CloneWithAllHeuristics();
                toAddExceptions.AddExceptions(r.Next(1,5));

                return new DeciderSpecies(toAddExceptions);
            }),
            ((sp) =>
            {
                ((HeuristicBuildingDecider)(sp.BaseDecider)).ExceptionRate = 0;
            }),
            numIterations);
        }


    }
}
