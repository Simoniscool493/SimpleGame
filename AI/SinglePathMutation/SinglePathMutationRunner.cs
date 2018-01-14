using SimpleGame.AI;
using SimpleGame.Games;
using SimpleGame.Metrics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame.Deciders.HeuristicBuilder
{
    class SinglePathMutationRunner
    {
        public DeciderSpecies BestSpecies;
        public HeuristicBuildingDecider BestDecider => ((HeuristicBuildingDecider)BestSpecies.BaseDecider);

        public bool MinimizeComplexity;
        public bool IncludePreviousBestWhenIteratingForwards;

        public int GenerationSize;
        public int TimesToTestPerSpecies;

        public int MaxHeuristicsToTake;

        private IDiscreteGameManager _game;
        private IDiscreteGameStateProvider _provider;

        public SinglePathMutationRunner(IDiscreteGameManager game, IDiscreteGameStateProvider provider,bool includePreviousBest,bool minimizeComplexity)
        {
            _game = game;
            _provider = provider;

            IncludePreviousBestWhenIteratingForwards = includePreviousBest;
            MinimizeComplexity = minimizeComplexity;
        }

        public void GreedyOptimize(int numIterations, Random r)
        {
            Console.WriteLine("\nMutating Heuristics\n");
            LookForBetterScore(1, r);

            for (int i = 0; i < numIterations; i++)
            {
                SystematicSearch(r);
                BestSpecies.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, BestDecider, TimesToTestPerSpecies);
                Console.WriteLine("Score: " + BestSpecies.Score + " Genes: " + BestSpecies.NumGenes + " Complexity: " + (BestDecider.TotalComplexity));
            }
        }

        public void Optimize(int numIterations,Random r)
        {
            double learningFactor = 0.5;

            for(int i=0;i<numIterations;i++)
            {
                var oldBestScore = BestSpecies.Score;

                Console.WriteLine("\nMutating Heuristics\n");
                LookForBetterScore((int)(50 * learningFactor), r); //50

                Console.WriteLine("\nRemoving Heuristics\n");
                RemoveHeuristics((int)(40 * learningFactor), r); //40 */

                Console.WriteLine("\nRemoving Conditions\n");
                RemoveConditions((int)(40 * learningFactor), r); //40 

                BestDecider.PostGenerationProcessing();

                foreach(var h in BestDecider.Heuristics)
                {
                    h.UseCount = 0;
                }

                if(oldBestScore == BestSpecies.Score)
                {
                    Console.WriteLine("Learning halted.");
                    SystematicSearch(r);
                }
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

                var oldBest = BestSpecies;


                BestSpecies = deciders.OrderBy(d => d.Score).Reverse().First();

                if (IncludePreviousBestWhenIteratingForwards && BestSpecies.Score < oldBest.Score)
                {
                    oldBest.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game,oldBest, TimesToTestPerSpecies);
                    BestSpecies = oldBest;
                }

                if (MinimizeComplexity)
                {
                    deciders.Add(oldBest);
                    var highestScores = deciders.Where(d => d.Score == BestSpecies.Score);

                    BestSpecies = highestScores.OrderBy(d => ((HeuristicBuildingDecider)(d.BaseDecider)).TotalComplexity).First();
                }

                postProcess?.Invoke(BestSpecies);

                Console.WriteLine("Score: " + BestSpecies.Score + " Genes: " + BestSpecies.NumGenes + " Complexity: " + (BestDecider.TotalComplexity));// + " Avg: " + deciders.Select(d=>d.Score).Average().ToString("0.0"));
                //Console.WriteLine("Score: " + ((CurrentBest.Score * 100.0f) / 244.0f).ToString("0.00") + " Genes: " + CurrentBest.NumGenes + " Complexity: " + (BestDecider.TotalComplexity));// + " Avg: " + deciders.Select(d=>d.Score).Average().ToString("0.0"));
            }
        }


        private void LookForBetterScore(int numIterations, Random r)
        {
            var mutationRate = (BestDecider.NumGenes == 0) ? 0.05 : ((double)r.Next(1,10)/ BestDecider.NumGenes);

            IterateChange((() =>
            {
                var mutated = (DeciderSpecies)BestSpecies.GetMutated(mutationRate, r);
                return mutated;
            }),
            null,
            numIterations);
        }

        private void RemoveHeuristics(int numIterations, Random r)
        {
            IterateChange((() => 
            {
                var toTake = r.Next(1, MaxHeuristicsToTake);

                var less = ((HeuristicBuildingDecider)BestSpecies.BaseDecider).CloneWithAllHeuristics();
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
                var toTake = r.Next(1,50);

                var less = ((HeuristicBuildingDecider)BestSpecies.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomConditions(toTake, r);
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
                var toAddExceptions = ((HeuristicBuildingDecider)BestSpecies.BaseDecider).CloneWithAllHeuristics();
                toAddExceptions.AddExceptions(r.Next(1,5));

                return new DeciderSpecies(toAddExceptions);
            }),
            ((sp) =>
            {
                ((HeuristicBuildingDecider)(sp.BaseDecider)).ExceptionRate = 0;
            }),
            numIterations);
        }

        private void SystematicSearch(Random r)
        {
            var baseDecider = ((HeuristicBuildingDecider)BestSpecies.BaseDecider);

            var usesToIndexesOrdered = GetOrderedUsesToIndexes();

            for(int i=0;i<baseDecider.Heuristics.Count;i++) //Remove each heuristic
            {
                var less = baseDecider.CloneWithAllHeuristics();
                less.Heuristics.RemoveAt(usesToIndexesOrdered.ElementAt(i).Value);

                var score = SimpleGameTester.SetRandomSuccessTesting(_game, less, TimesToTestPerSpecies);

                if(score>BestSpecies.Score)
                {
                    BestSpecies = new DeciderSpecies(less);
                    return;
                }
            }

            Console.WriteLine("Heuristic removal insufficent. Removing extraneous complexity...");

            for (int i = 0; i < 10; i++)
            {
                var oldHeurs = BestDecider.Heuristics.Count;
                var oldScore = BestSpecies.Score;

                RemoveHeuristics(10, r);

                if (BestSpecies.Score > oldScore)
                {
                    return;
                }

                if (oldHeurs == BestDecider.Heuristics.Count)
                {
                    break;
                }
            }

            for (int i=0;i<10;i++)
            {
                var oldConds = BestDecider.NumConditions;
                var oldScore = BestSpecies.Score;

                RemoveConditions(10, r);

                if (BestSpecies.Score>oldScore)
                {
                    return;
                }

                if (oldConds == BestDecider.NumConditions)
                {
                    break;
                }
            }

            baseDecider = ((HeuristicBuildingDecider)BestSpecies.BaseDecider);
            usesToIndexesOrdered = GetOrderedUsesToIndexes();

            for (int i = 0; i < baseDecider.Heuristics.Count; i++) //Remove each condition
            {
                var indexToCheck = usesToIndexesOrdered.ElementAt(i).Value;
                var numConditions = baseDecider.Heuristics.ElementAt(indexToCheck).Conditions.Count;

                Console.WriteLine(i + "/" + baseDecider.Heuristics.Count);
                for(int j=0;j<numConditions;j++)
                {
                    var less = baseDecider.CloneWithAllHeuristics();

                    var heuristicToCheck = less.Heuristics.ElementAt(indexToCheck);
                    heuristicToCheck.Conditions.RemoveAt(j);

                    var score = SimpleGameTester.SetRandomSuccessTesting(_game, less, TimesToTestPerSpecies);

                    if (score > BestSpecies.Score)
                    {
                        BestSpecies = new DeciderSpecies(less);
                        var score2 = SimpleGameTester.SetRandomSuccessTesting(_game, BestSpecies.BaseDecider, TimesToTestPerSpecies);

                        return;
                    }

                }
            }

            Console.WriteLine("not found");
        }

        private List<KeyValuePair<int,int>> GetOrderedUsesToIndexes()
        {
            SimpleGameTester.SetRandomSuccessTesting(_game, BestSpecies, TimesToTestPerSpecies);
            var baseDecider = ((HeuristicBuildingDecider)BestSpecies.BaseDecider);

            List<KeyValuePair<int, int>> usesToIndexes = new List<KeyValuePair<int, int>>();

            for (int h = 0; h < baseDecider.Heuristics.Count; h++)
            {
                usesToIndexes.Add(new KeyValuePair<int, int>(baseDecider.Heuristics.ElementAt(h).UseCount, h));
            }

            var usesToIndexesOrdered = usesToIndexes.OrderBy(uti => uti.Key).Reverse().ToList();

            return usesToIndexesOrdered;
        }
    }
}
