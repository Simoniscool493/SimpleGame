using SimpleGame.Deciders;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;
using SimpleGame.Metrics;
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

        public GeneticAlgorithmRunner(int numGenerations,int numToKill,int numInGeneration,int numOfTimesToTestASpecies,double mutationRate,DeciderType deciderType)
        {
            _numGenerations = numGenerations;
            _numToKillPerGeneration = numToKill;
            _numInGeneration = numInGeneration;
            _numOfTimesToTestASpecies = numOfTimesToTestASpecies;
            _mutationRate = mutationRate;
            _deciderType = deciderType;
        }

        
        public IDiscreteDecider Train(IDiscreteGameManager game,IDiscreteGameStateProvider provider,bool showProgress,int demonstrateEveryXIterations)
        {
            var trainableState = provider.GetStateForNextGeneration();
            var r = new Random();

            Generation currentGeneration = new Generation(_numInGeneration,_mutationRate,r);

            currentGeneration.PopulateWithRandoms(game.IOInfo,_deciderType);

            for (int generationCounter=0; generationCounter < _numGenerations; generationCounter++)
            {
                var avg = RunGeneration(game, trainableState,currentGeneration);

                if(showProgress && ((generationCounter % demonstrateEveryXIterations) == 0) && generationCounter!=0)
                {
                    game.Demonstrate(currentGeneration.GetBestSpecies(), provider.GetStateForDemonstration());
                    //trainableState.Reset();
                }

                Console.WriteLine("Generation " + (generationCounter + 1) + ". Best " + currentGeneration.GetBestSpecies().Score + " Avg: " + avg);
            }

            return currentGeneration.GetBestSpecies();
        }

        protected virtual int RunGeneration(IDiscreteGameManager game,IDiscreteGameState state,Generation currentGeneration)
        {
            currentGeneration.ScoreGeneration(game, state,_numOfTimesToTestASpecies);
            currentGeneration.Kill(_numToKillPerGeneration);
            currentGeneration.Multiply();

            return currentGeneration.GetAverageScore();
        }

    }
}
