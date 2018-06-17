using SimpleGame.Deciders;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;
using SimpleGame.Metrics;
using System.Linq;
using System;
using System.IO;
using System.Text;

namespace SimpleGame.AI.GeneticAlgorithm
{
    public class GeneticAlgorithmRunner : IDiscreteDecisionModel
    {
        private int _numGenerations;
        private int _numToKillPerGeneration;
        private int _numInGeneration;
        private int _numOfTimesToTestASpecies;
        private double _mutationRate;
        private DiscreteDeciderType _deciderType;
        private bool _printBasicInfo;
        protected bool _earlyStopFlag = false;
        protected int _generationCounter;

        public GeneticAlgorithmRunner(int numGenerations,int numToKill,int numInGeneration,int numOfTimesToTestASpecies,double mutationRate,DiscreteDeciderType deciderType)
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

            var trainableState = provider.GetStateForTraining(1);
            //??? TODO
            var r = new Random();

            Generation currentGeneration = new Generation(_numInGeneration,_mutationRate ,r);

            currentGeneration.PopulateWithRandoms(game.IOInfo,_deciderType);

            for (_generationCounter = 0; _generationCounter < _numGenerations && !_earlyStopFlag; _generationCounter++)
            {
                var avg = RunGeneration(game, trainableState,currentGeneration);
                currentGeneration.MutationRate = 5.0/(currentGeneration.BestSpecies.NumGenes);

                if (showGameProgress && ((_generationCounter % demonstrateEveryXIterations) == 0) && _generationCounter != 0)
                {
                    PrintBasicInfo(currentGeneration);
                    //game.Demonstrate(currentGeneration.BestSpecies, provider.GetStateForDemonstration());
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

                var path = Path.Combine(SimpleGameLauncher.LogsPath,"GenAlgLogs", $"{_generationCounter}_({currentGeneration.BestSpecies.Score})");
            }

            _earlyStopFlag = false;
            sw?.Close();
            return currentGeneration.BestSpecies;
        }

        private StreamWriter sw;

        private void WriteGenerationRaw(Generation g)
        {
            if (sw == null) { sw = new StreamWriter(Path.Combine(SimpleGameLauncher.LogsPath, "GenAlgLogs", $"{_deciderType.ToString()}")); }

            StringBuilder sb = new StringBuilder();
            sb.Append("Generation " + _generationCounter + "\n");
            foreach(var sp in g.ThisGeneration)
            {
                sb.Append("\n\n" + sp.GetFullStringRepresentation());
            }

            sb.Append("\n\n\n");

            sw.WriteLine(sb.ToString());
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
            string info = $"{_generationCounter + 1}. Best {currentGeneration.BestSpecies.Score} Avg:{currentGeneration.AverageScore} Worst:{currentGeneration.WorstScore}";

            info = info + " AvgGenes: " + currentGeneration.ThisGeneration.Select(sp => sp.NumGenes).Average().ToString("0.00");
            /*string arrayAsString = "";

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

            info = info + '\t' + arrayAsString;*/

            Console.WriteLine(info);
        }

    }
}
