using log4net;
using SimpleGame.AI;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
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

        public IDiscreteDecider BestDecider => (BestSpecies.BaseDecider);
        public HeuristicBuildingDecider BestDeciderAsHeuristic => ((HeuristicBuildingDecider)BestSpecies.BaseDecider);

        public bool MinimizeComplexity;
        public bool IncludePreviousBestWhenIteratingForwards;
        public bool SetTheRandomSeed;

        public int GenerationSize;
        public int TimesToTestPerSpecies;

        public int MaxHeuristicsToTake;

        private IDiscreteGameManager _game;
        private IDiscreteGameStateProvider _provider;
        private ILog _logger;
        private int totalCounter;

        private enum StatusTypeNumber
        {
            OverallStatus = 0,
            Mutate,
            RemoveHeuristics,
            RemoveConditions,
            GreedySearch,
            AddExceptions
        }


    public SinglePathMutationRunner(ILog log,IDiscreteGameManager game, IDiscreteGameStateProvider provider,bool includePreviousBest,bool minimizeComplexity,bool setTheRandomSeed)
        {
            _logger = log;
            _game = game;
            _provider = provider;

            IncludePreviousBestWhenIteratingForwards = includePreviousBest;
            MinimizeComplexity = minimizeComplexity;
            SetTheRandomSeed = setTheRandomSeed;
            totalCounter = 0;

        }

        int prevBestTemp;

        public void Optimize(int numIterations,double learningFactor,Random r)
        {
            Log("MutationRunnerConfig," + GenerationSize + "," + MaxHeuristicsToTake + "," +TimesToTestPerSpecies + "," + numIterations + "," +learningFactor);

            prevBestTemp = 0;
            double targetComplexity = 0;

            for(int i=0;i<numIterations;i++)
            {
                var oldBestScore = BestSpecies.Score;
                var oldComplexity = BestSpecies.TotalComplexity;
                var oldBest = BestSpecies;

                if(oldComplexity< targetComplexity || targetComplexity == 0)
                {
                    targetComplexity = oldComplexity * 0.9;
                }

                Log("\nMutating Heuristics\n");
                MutateHeuristics((int)(50 * learningFactor), r); //50

                Log("\nRemoving Heuristics\n");
                RemoveHeuristics((int)(40 * learningFactor), r,false); //40 */

                Log("\nRemoving Conditions\n");
                RemoveConditions((int)(40 * learningFactor), r,false); //40 


                Log("\nLowering Complexity\n");

                while(true)
                {
                    RemoveHeuristics((int)(50 * learningFactor), r, true); //40 */
                    RemoveConditions((int)(50 * learningFactor), r, true); //40 

                    if(i<2 || BestSpecies.TotalComplexity < 5000) { break; }

                    var currentComplexity = BestSpecies.TotalComplexity;

                    if(currentComplexity <= targetComplexity)
                    {
                        break;
                    }
                }

                BestDecider.PostGenerationProcessing();

                if(oldBestScore == BestSpecies.Score)
                {
                    Log("Score not increased at all in one pass. Beginning systematic search.");

                    var recordedPrevScore = BestSpecies.Score;
                    var testedPrevScore = (int)SimpleGameTester.SetRandomSuccessTesting(_game, BestDecider, TimesToTestPerSpecies);

                    if(recordedPrevScore != testedPrevScore)
                    {
                        throw new Exception();
                    }

                    SystematicSearch(r);

                    var newScore = (int)SimpleGameTester.SetRandomSuccessTesting(_game, BestDecider, TimesToTestPerSpecies);

                    if(testedPrevScore > newScore)
                    {
                        throw new Exception();
                    }
                }

                if(prevBestTemp>BestSpecies.Score)
                {
                    //throw new Exception();
                    BestSpecies = oldBest;
                }
                prevBestTemp = BestSpecies.Score;

                PrintStatus(StatusTypeNumber.OverallStatus);
            }
        }

        public void GreedyOptimize(int numIterations, Random r)
        {
            Log("\nGreedyOptimizing\n");
            MutateHeuristics(1, r);

            for (int i = 0; i < numIterations; i++)
            {
                SystematicSearch(r);
                BestSpecies.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, BestDecider, TimesToTestPerSpecies);
                PrintStatus(StatusTypeNumber.GreedySearch);
            }
        }

        private void PrintStatus(StatusTypeNumber statusTypeNumber)
        {
            var score = "Score: " + BestSpecies.Score;

            if(TimesToTestPerSpecies>1)
            {
                var avgScore = (int)(((double)BestSpecies.Score) / ((double)TimesToTestPerSpecies));
                score = score + " Avg: " + avgScore;
            }

            string message = score + " Genes: " + BestSpecies.NumGenes + " Complexity: " + (BestDecider.TotalComplexity);
            Console.WriteLine(message);
            _logger.Info("RawData," + totalCounter++ + "," + ((int)statusTypeNumber) +  "," + BestSpecies.Score + "," + BestSpecies.NumGenes + "," + BestDecider.TotalComplexity);
        }

        private void Log(string s)
        {
            Console.WriteLine(s);
            _logger.Info(s);
        }

        private void IterateChange(StatusTypeNumber statusTypeNumber,Func<DeciderSpecies> processSpecies, Action<DeciderSpecies> postProcess, int numIterations,bool prioritizeLoweringComplexityOverScore)
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

                if(prioritizeLoweringComplexityOverScore)
                {
                    var theList = deciders.Where(d => (d.Score >= oldBest.Score)).OrderBy(d => d.TotalComplexity);

                    if(theList.Count() > 0)
                    {
                        var lowestComplexity = theList.First();

                        if(lowestComplexity.TotalComplexity <= oldBest.TotalComplexity)
                        {
                            BestSpecies = theList.First();
                        }
                    }

                    if(BestSpecies.TotalComplexity > (oldBest.TotalComplexity + 100))
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    BestSpecies = deciders.OrderBy(d => d.Score).Reverse().First();

                    if (IncludePreviousBestWhenIteratingForwards && BestSpecies.Score < oldBest.Score)
                    {
                        oldBest.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, oldBest, TimesToTestPerSpecies);
                        BestSpecies = oldBest;
                    }

                    if (MinimizeComplexity)
                    {
                        deciders.Add(oldBest);
                        var highestScores = deciders.Where(d => d.Score == BestSpecies.Score);

                        BestSpecies = highestScores.OrderBy(d => ((HeuristicBuildingDecider)(d.BaseDecider)).TotalComplexity).First();
                    }
                }

                postProcess?.Invoke(BestSpecies);

                PrintStatus(statusTypeNumber);
            }
        }

        private void MutateHeuristics(int numIterations, Random r)
        {
            var mutationRate = (BestDecider.NumGenes == 0) ? 0.05 : ((double)r.Next(1,10)/ BestDecider.NumGenes);

            IterateChange(StatusTypeNumber.Mutate,(() =>
            {
                var mutated = (DeciderSpecies)BestSpecies.GetMutated(mutationRate, r);
                return mutated;
            }),
            null,
            numIterations,
            false);
        }

        private void RemoveHeuristics(int numIterations, Random r,bool prioritizeLoweringComplexityOverScore)
        {
            IterateChange(StatusTypeNumber.RemoveHeuristics,(() => 
            {
                var toTake = r.Next(1, MaxHeuristicsToTake);

                var less = ((HeuristicBuildingDecider)BestSpecies.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomHeuristics(toTake);
                return new DeciderSpecies(less);
            }),
            null, 
            numIterations,
            prioritizeLoweringComplexityOverScore);
        }

        private void RemoveConditions(int numIterations,Random r, bool prioritizeLoweringComplexityOverScore)
        {
            IterateChange(StatusTypeNumber.RemoveConditions,(() =>
            {
                var toTake = r.Next(1,50);

                var less = ((HeuristicBuildingDecider)BestSpecies.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomConditions(toTake, r);
                return new DeciderSpecies(less);
            }),
            null,
            numIterations,
            prioritizeLoweringComplexityOverScore);
        }

        private void AddExceptions(int numIterations, Random r)
        {
            IterateChange(StatusTypeNumber.AddExceptions,
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
            numIterations,
            false);
        }

        public List<HeuristicBuildingDecider> GetListOfChangedDeciders(List<HeuristicBuildingDecider> listOfOriginals)
        {
            var possibleValues = ((DiscreteDataPayloadInfo)(listOfOriginals.First().IOInfo.OutputInfo)).AllPossibleValues();
            var output = new List<HeuristicBuildingDecider>();

            foreach(var decider in listOfOriginals)
            {
                var usesToIndexesOrdered = GetOrderedUsesToIndexes(new DeciderSpecies(decider));

                for (int i = 0; i < decider.Heuristics.Count; i++)
                {
                    for (int j = 0; j < possibleValues.Count; j++)
                    {
                        var deciderWithOneChangedHeuristic = decider.CloneWithAllHeuristics();
                        deciderWithOneChangedHeuristic.Heuristics.ElementAt(usesToIndexesOrdered.ElementAt(i).Value).ExpectedOutput = possibleValues.ElementAt(j).SingleItem;

                        output.Add(deciderWithOneChangedHeuristic);
                    }
                }
            }

            return output;
        }


        private void SystematicSearch(Random r)
        {
            Log("1: best score is " + BestSpecies.Score);
            var originalBaseDecider = ((HeuristicBuildingDecider)BestSpecies.BaseDecider);
            var usesToIndexesOrdered = GetOrderedUsesToIndexes(BestSpecies);

            Log("Checking single heursitcs-by-one...");

            var listToTest = new List<HeuristicBuildingDecider> { (HeuristicBuildingDecider)BestSpecies.BaseDecider };

            NewLevel:

            listToTest = GetListOfChangedDeciders(listToTest);
            var betterScores = new Dictionary<HeuristicBuildingDecider, int>();

            foreach(var decider in listToTest)
            {
                var score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, decider, TimesToTestPerSpecies);

                if (score > BestSpecies.Score)
                {
                    betterScores[decider] = score;
                }
            }

            if(betterScores.Any())
            {
                var ordered = betterScores.OrderBy(kvp => kvp.Value).Reverse();
                var newDecider = ordered.First().Key;

                BestSpecies = new DeciderSpecies(newDecider);
                BestSpecies.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, newDecider, TimesToTestPerSpecies);

                Log("2: best score is " + BestSpecies.Score);
                return;
            }
            else
            {
                Console.WriteLine("Heuristic change insufficent. Going one step farther.");
                return;

                goto NewLevel;

            }

            return;













            Console.WriteLine("Removing extraneous complexity...");

            for (int i = 0; i < 10; i++)
            {
                var numberOfHeuristicsBeforeRemovingHeuristics = BestDeciderAsHeuristic.Heuristics.Count;
                var scoreBeforeRemovingHeuristics = BestSpecies.Score;

                RemoveHeuristics(10, r,true);

                if (BestSpecies.Score > scoreBeforeRemovingHeuristics)
                {
                    Console.WriteLine("3: best score is " + BestSpecies.Score);
                    return;
                }

                if (numberOfHeuristicsBeforeRemovingHeuristics == BestDeciderAsHeuristic.Heuristics.Count) { break; }
            }

            for (int i=0;i<10;i++)
            {
                var oldConds = BestDeciderAsHeuristic.NumConditions;
                var oldScore = BestSpecies.Score;

                RemoveConditions(10, r,true);

                if (BestSpecies.Score>oldScore)
                {
                    Console.WriteLine("4: best score is " + BestSpecies.Score);
                    return;
                }

                if (oldConds == BestDeciderAsHeuristic.NumConditions)
                {
                    break;
                }
            }

            originalBaseDecider = ((HeuristicBuildingDecider)BestSpecies.BaseDecider);
            usesToIndexesOrdered = GetOrderedUsesToIndexes(BestSpecies);

            /*for (int i = 0; i < originalBaseDecider.Heuristics.Count; i++) //Remove each condition
            {
                var indexToCheck = usesToIndexesOrdered.ElementAt(i).Value;
                var numConditions = originalBaseDecider.Heuristics.ElementAt(indexToCheck).Conditions.Count;

                Console.WriteLine(i + "/" + originalBaseDecider.Heuristics.Count);
                for(int j=0;j<numConditions;j++)
                {
                    var less = originalBaseDecider.CloneWithAllHeuristics();

                    var heuristicToCheck = less.Heuristics.ElementAt(indexToCheck);
                    heuristicToCheck.Conditions.RemoveAt(j);

                    var score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, less, TimesToTestPerSpecies);

                    if(score!=BestSpecies.Score)
                    {
                        Console.Write(" " + score);
                    }

                    if (score > BestSpecies.Score)
                    {
                        BestSpecies = new DeciderSpecies(less);
                        BestSpecies.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, BestSpecies.BaseDecider, TimesToTestPerSpecies);

                        Console.WriteLine();
                        Console.WriteLine("5: best score is " + BestSpecies.Score);
                        return;
                    }
                }

                Console.WriteLine();
            }*/

            Console.WriteLine("not found");
        }

        private List<KeyValuePair<int,int>> GetOrderedUsesToIndexes(DeciderSpecies sp)
        {
            SimpleGameTester.SetRandomSuccessTesting(_game, sp, TimesToTestPerSpecies);
            var baseDecider = ((HeuristicBuildingDecider)sp.BaseDecider);

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
