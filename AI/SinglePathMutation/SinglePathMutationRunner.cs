using log4net;
using SimpleGame.AI;
using SimpleGame.AI.SinglePathMutation;
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
        #region bests

        public DeciderSpecies BestSpecies
        {
            get { return _bestSpecies; }
            set
            {
                var allegedScore = value.Score;
                var realScore = (int)SimpleGameTester.SetRandomSuccessTesting(_game, value, _numRandomSeeds,_baseRandomSeed);

                if(allegedScore!=realScore)
                {
                    throw new Exception();
                }

                _bestSpecies = value;

                if (_bestSpecies.Score > _bestSpeciesOverall.Score || (_bestSpecies.Score==_bestSpeciesOverall.Score && _bestSpecies.TotalComplexity < _bestSpeciesOverall.TotalComplexity))
                {
                    BestSpeciesOverall = _bestSpecies;
                }
            }
        }
        private DeciderSpecies _bestSpecies;

        public DeciderSpecies BestSpeciesOverall
        {
            get { return _bestSpeciesOverall; }
            set
            {
                if (_bestSpeciesOverall != null && value.Score < _bestSpeciesOverall.Score)
                {
                    throw new Exception();
                }

                _bestSpeciesOverall = value;
            }
        }

        private int _fullNumLoops;
        private DeciderSpecies _bestSpeciesOverall;
        public Stack<DeciderSpecies> previousBests;

        #endregion

        private IDiscreteGameManager _game;
        private IDiscreteGameStateProvider _provider;
        private SystematicSearcher _systematicSearcher;
        private SystematicSimplifier _systematicSimplifier;


        private ILog _logger;
        private Random _r;
        private int _totalCounter;

        private int _generationSize;
        private int _numRandomSeeds;
        private int _baseRandomSeed;
        private int _maxHeuristicsToTake;
        private int targetComplexity;
        private int noChangeCount;
        private int _numIterationsBeforeRegression;
        private int _numTimesToTryIteration;

        private bool _shouldLogToConsole;
        private bool _shouldLogToFile;

        private const double complexityLoweringFactor = 0.9d;
        private int preferrredMinComplexity;

        #region initialization

        public SinglePathMutationRunner
        (
            ILog log,
            Random r,
            IDiscreteGameManager game, 
            DeciderSpecies initialSpecies,
            IDiscreteGameStateProvider provider,
            bool shouldLogToConsole,
            bool shouldLogToFile,
            int numRandomSeeds,
            int numIterationsBeforeRegression,
            int numTimesToTryIteration,
            int generationSize,
            int maxHeuristicsToTake,
            int baseRandomSeed = 0)
        {
            _logger = log;
            _r = r;
            _game = game;
            _bestSpecies = initialSpecies;
            _bestSpeciesOverall = initialSpecies;
            _provider = provider;

            _shouldLogToConsole = shouldLogToConsole;
            _shouldLogToFile = shouldLogToFile;

            _numRandomSeeds = numRandomSeeds;
            _numIterationsBeforeRegression = numIterationsBeforeRegression;
            _numTimesToTryIteration = numTimesToTryIteration;
            _generationSize = generationSize;
            _maxHeuristicsToTake = maxHeuristicsToTake;

            _totalCounter = 0;
            previousBests = new Stack<DeciderSpecies>();

            _systematicSearcher = new SystematicSearcher(_game,_numRandomSeeds,baseRandomSeed,Log);
            _systematicSimplifier = new SystematicSimplifier(_game, _numRandomSeeds,baseRandomSeed);

            _baseRandomSeed = baseRandomSeed;
        }

        #endregion

        public void Optimize(int numIterations,double learningFactor, int preferredMaxComplexity,int preferredMinComplexity)
        {
            this.preferrredMinComplexity = preferredMinComplexity;

            Log("MutationRunnerConfig," + _generationSize + "," + _maxHeuristicsToTake + "," + _numRandomSeeds + "," + _numIterationsBeforeRegression + "," + learningFactor);
            DeciderSpecies oldBest = BestSpecies;

            for (int i=0;i<numIterations;i++)
            {
                Alter(i,numIterations, learningFactor, preferredMaxComplexity);
                PrintStatus(StatusTypeNumber.OverallStatus);

                if(BestSpecies.Score>oldBest.Score)
                {
                    previousBests.Push(BestSpecies);
                    noChangeCount = 0;
                }
                else
                {
                    noChangeCount++;
                    if (noChangeCount >= _numIterationsBeforeRegression)
                    {
                        Regress();
                        noChangeCount = 0;
                    }
                }

                oldBest = BestSpecies;
                _fullNumLoops++;
            }
        }

        private void Regress()
        {
            if(previousBests.Count>0)
            {
                Log("Pushing back");
                BestSpecies = previousBests.Pop();
                BestSpecies.TimesTried++;
                if (BestSpecies.TimesTried > _numTimesToTryIteration)
                {
                    Regress();
                }
            }
            else
            {
                //BestSpecies = new DeciderSpecies(new HeuristicBuildingDecider(_r, BestSpecies.IOInfo,((HeuristicBuildingDecider)BestSpecies.BaseDecider).NumConditionsToBuildFrom));
            }
        }

        private void Alter(int i,int numIterations, double learningFactor, int preferredMaxComplexity)
        {
            var oldBest = BestSpecies;

            if (targetComplexity == 0 || oldBest.TotalComplexity < targetComplexity)
            {
                targetComplexity = (int)(oldBest.TotalComplexity * complexityLoweringFactor);
            }

            Log("\nMutating Heuristics\n");
            MutateHeuristics((int)(50 * learningFactor)); //50

            Log("\nRemoving Heuristics\n");
            RemoveHeuristics((int)(40 * learningFactor), false); //40 */

            Log("\nRemoving Conditions\n");
            RemoveConditions((int)(40 * learningFactor), false); //40 

            Log("\nLowering Complexity\n");
            targetComplexity = 0;

            if(BestSpecies.TotalComplexity>preferredMaxComplexity)
            {
                LowerComplexity(i, (int)(30 * learningFactor), preferredMaxComplexity);
            }

            /*while(BestSpecies.TotalComplexity>10)
            {
                //RemoveConditions(1, true); //40 
                RemoveHeuristics(1, true); //40 
            }


            BestSpecies = _systematicSimplifier.SimplifySpeciesAsMuchAsPossible(BestSpecies);

            BestSpecies.PostGenerationProcessing();*/

            if (oldBest.Score == BestSpecies.Score)
            {
                //Log("Score not increased at all in one pass. Beginning systematic condition erasure.");
                //SystematicSearch(true);
            }

            if (BestSpecies.Score < oldBest.Score)
            {
                throw new Exception();
            }
        }

        private void IterateChange(StatusTypeNumber statusTypeNumber, Func<DeciderSpecies> processSpecies, Action<DeciderSpecies> postProcess, int numIterations, bool prioritizeLoweringComplexityOverScore)
        {
            for (int i = 0; i < numIterations; i++)
            {
                SingleChange(processSpecies, postProcess, prioritizeLoweringComplexityOverScore);
                PrintStatus(statusTypeNumber);
            }
        }

        private void SingleChange(Func<DeciderSpecies> processSpecies, Action<DeciderSpecies> postProcess, bool prioritizeLoweringComplexityOverScore)
        {
            var deciders = new List<DeciderSpecies>() { BestSpecies };

            while (deciders.Count() < _generationSize)
            {
                DeciderSpecies changedDecider = processSpecies();
                var heuristicAvg = SimpleGameTester.SetRandomSuccessTesting(_game, changedDecider, _numRandomSeeds,_baseRandomSeed);

                changedDecider.Score = (int)heuristicAvg;
                deciders.Add(changedDecider);
            }

            IOrderedEnumerable<DeciderSpecies> theList;

            if (prioritizeLoweringComplexityOverScore)
            {
                theList = deciders.Where(d => (d.Score >= BestSpecies.Score)).OrderBy(d => d.TotalComplexity);
            }
            else
            {
                var topScore = deciders.Max(d => d.Score);
                theList = deciders.Where(d => (d.Score == topScore)).OrderBy(d => d.TotalComplexity);
            }

            BestSpecies = theList.First();
            postProcess?.Invoke(BestSpecies);
        }

        public void LowerComplexity(int i,int numIterations,int preferredMaxComplexity)
        {
            while (true)
            {
                RemoveHeuristics(numIterations, true); //40 */
                RemoveConditions(numIterations, true); //40 

                if (_fullNumLoops < 2 || BestSpecies.TotalComplexity < preferredMaxComplexity) { break; }

                if (BestSpecies.TotalComplexity <= targetComplexity)
                {
                    break;
                }
            }
        }

        private void SystematicSearch(bool shouldLog)
        {
            var newBest = _systematicSearcher.SystematicSearch(BestSpecies, _r,shouldLog);

            if(newBest!=null)
            {
                BestSpecies = newBest;
            }
        }

        #region standardAlterations

        private void MutateHeuristics(int numIterations)
        {
            var mutationRate = (BestSpecies.NumGenes == 0) ? 0.05 : ((double)_r.Next(1,10)/ BestSpecies.NumGenes);

            IterateChange(StatusTypeNumber.Mutate,(() =>
            {
                var mutated = (DeciderSpecies)BestSpecies.GetMutated(mutationRate, _r);
                return mutated;
            }),
            null,
            numIterations,
            false);
        }

        private void RemoveHeuristics(int numIterations,bool prioritizeLoweringComplexityOverScore)
        {
            IterateChange(StatusTypeNumber.RemoveHeuristics,(() => 
            {
                var toTake = _r.Next(1, _maxHeuristicsToTake);

                var less = ((HeuristicBuildingDecider)BestSpecies.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomHeuristics(toTake);
                return new DeciderSpecies(less);
            }),
            null, 
            numIterations,
            prioritizeLoweringComplexityOverScore);
        }

        private void RemoveConditions(int numIterations, bool prioritizeLoweringComplexityOverScore)
        {
            IterateChange(StatusTypeNumber.RemoveConditions,(() =>
            {
                var toTake = _r.Next(1,50);

                var less = ((HeuristicBuildingDecider)BestSpecies.BaseDecider).CloneWithAllHeuristics();
                less.RemoveRandomConditions(toTake, _r);
                return new DeciderSpecies(less);
            }),
            null,
            numIterations,
            prioritizeLoweringComplexityOverScore);
        }

        #endregion


        public void GreedyOptimize(int numIterations, Random r)
        {
            Log("\nGreedyOptimizing\n");
            MutateHeuristics(1);

            for (int i = 0; i < numIterations; i++)
            {
                SystematicSearch(false);
                BestSpecies.Score = (int)SimpleGameTester.SetRandomSuccessTesting(_game, BestSpecies, _numRandomSeeds,_baseRandomSeed);
                PrintStatus(StatusTypeNumber.GreedySearch);
            }
        }

        #region logging

        private void PrintStatus(StatusTypeNumber statusTypeNumber)
        {
            var score = "Score: " + BestSpecies.Score;
            double unseenScore = 0 ;

            if(BestSpeciesOverall.TotalComplexity > preferrredMinComplexity)
            {
                ((HeuristicBuildingDecider)BestSpeciesOverall.BaseDecider).ShouldMakeNewHeuristics = false;
            }

            if(_numRandomSeeds >1)
            {
                unseenScore = SimpleGameTester.UnsetRandomSuccessTesting(_game, BestSpeciesOverall, 100);
                ((HeuristicBuildingDecider)BestSpeciesOverall.BaseDecider).ShouldMakeNewHeuristics = true; 
            }

            //TODO

            if (_numRandomSeeds > 1)
            {
                var avgScore = (int)(((double)BestSpecies.Score) / ((double)_numRandomSeeds));
                score = score + " Avg: " + avgScore;
            }

            string message = score + " Genes: " + BestSpecies.NumGenes + " Complexity: " + (BestSpecies.TotalComplexity) + " Best: " + BestSpeciesOverall.Score + " Depth: " + previousBests.Count;
            
            if(_numRandomSeeds>1)
            {
                message = message + " Unseen: " + unseenScore;
                //message = message + " Rand: " + SimpleGameTester.UnsetRandomSuccessTesting(_game, new RandomDiscreteDecider(_r, BestSpeciesOverall.IOInfo), 100);
            }

            if (_shouldLogToConsole)
            {
                Console.WriteLine(message);
            }
            if (_shouldLogToFile)
            {
                _logger.Info("RawData," + _totalCounter++ + "," + ((int)statusTypeNumber) + "," + BestSpecies.Score + "," + BestSpecies.NumGenes + "," + BestSpecies.TotalComplexity);
            }
        }

        private void Log(string s)
        {
            if (_shouldLogToConsole)
            {
                Console.WriteLine(s);
            }
            if (_shouldLogToFile)
            {
                _logger.Info(s);
            }
        }

        #endregion
    }
}
