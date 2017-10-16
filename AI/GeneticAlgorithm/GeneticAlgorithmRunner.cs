using SimpleGame.Deciders;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;
using SimpleGame.Metrics;
using System.Linq;
using System;

namespace SimpleGame.AI.GeneticAlgorithm
{
    public class GeneticAlgorithmRunner : IDiscreteDecisionModel
    {
        private int _numGenerations;
        private int _numToKillPerGeneration;
        private int _numInGeneration;
        private int _numOfTimesToTestASpecies;
        private double _mutationRate;
        private DeciderType _deciderType;
        private bool _printBasicInfo;
        protected bool _earlyStopFlag = false;
        protected int _generationCounter;

        public GeneticAlgorithmRunner(int numGenerations,int numToKill,int numInGeneration,int numOfTimesToTestASpecies,double mutationRate,DeciderType deciderType)
        {
            _numGenerations = numGenerations;
            _numToKillPerGeneration = numToKill;
            _numInGeneration = numInGeneration;
            _numOfTimesToTestASpecies = numOfTimesToTestASpecies;
            _mutationRate = mutationRate;
            _deciderType = deciderType;
        }

        private int prevAvg = 0;
        private int noChangeCounter = 0;

        public IDiscreteDecider Train(IDiscreteGameManager game,IDiscreteGameStateProvider provider,bool showGameProgress,bool printBasicInfo,int demonstrateEveryXIterations)
        {
            _printBasicInfo = printBasicInfo;

            var trainableState = provider.GetStateForNextGeneration();
            var r = new Random();

            Generation currentGeneration = new Generation(_numInGeneration,_mutationRate ,r);

            currentGeneration.PopulateWithRandoms(game.IOInfo,_deciderType);

            for (_generationCounter = 0; _generationCounter < _numGenerations && !_earlyStopFlag; _generationCounter++)
            {
                //var mutationRateModifier = 100;//(noChangeCounter / 100);
                //currentGeneration.MutationRate = _mutationRate + mutationRateModifier;

                var avg = RunGeneration(game, trainableState,currentGeneration);

                if (showGameProgress && ((_generationCounter % demonstrateEveryXIterations) == 0) && _generationCounter != 0)
                {
                    game.Demonstrate(currentGeneration.BestSpecies, provider.GetStateForDemonstration());
                    //trainableState.Reset();
                }

                if(avg <= prevAvg)
                {
                    noChangeCounter++;
                }
                else
                {
                    noChangeCounter = 0;
                }

                prevAvg = avg;
            }

            _earlyStopFlag = false;
            return currentGeneration.BestSpecies;
        }

        protected virtual int RunGeneration(IDiscreteGameManager game,IDiscreteGameState state,Generation currentGeneration)
        {
            currentGeneration.ScoreGeneration(game, state,_numOfTimesToTestASpecies);
            currentGeneration.Kill(_numToKillPerGeneration);

            if (_printBasicInfo) { PrintBasicInfo(currentGeneration); }

            currentGeneration.Multiply();

            return currentGeneration.AverageScore;
        }

        private void PrintBasicInfo(Generation currentGeneration)
        {
            var basicinfo = $"{_generationCounter + 1}. Best {currentGeneration.BestSpecies.Score} Avg:{currentGeneration.AverageScore} Worst:{currentGeneration.WorstScore}";

            string arrayAsString = "";

            foreach (int score in currentGeneration.ThisGeneration.Select(sp => sp.Score).OrderBy(i => i).Reverse())
            {
                if (score < 100)
                {
                    arrayAsString = arrayAsString + " ";
                }
                if (score < 10)
                {
                    arrayAsString = arrayAsString + " ";
                }

                arrayAsString = arrayAsString + " " + score;
            }

            Console.WriteLine(basicinfo + '\t' + arrayAsString);
        }

    }
}
