using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;
using System;

namespace SimpleGame.AI.GeneticAlgorithm
{
    public class GeneticAlgorithmRunner : IDiscreteDecisionModel
    {
        private int _numGenerations;
        private int _numToKillPerGeneration;
        private int _numInGeneration;
        private double _mutationRate;
        private bool _IsLazy;

        public GeneticAlgorithmRunner(int numGenerations,int numToKill,int numInGeneration,double mutationRate,bool isLazy)
        {
            _numGenerations = numGenerations;
            _numToKillPerGeneration = numToKill;
            _numInGeneration = numInGeneration;
            _mutationRate = mutationRate;
            _IsLazy = isLazy;
        }

        public IDiscreteDecider Train(IDiscreteGameManager game,IDiscreteGameStateProvider provider,bool showProgress,int demonstrateEveryXIterations)
        {
            var trainableState = provider.GetStateForNextGeneration();
            var r = new Random();

            Generation currentGeneration = new Generation(_numInGeneration,_mutationRate);

            currentGeneration.PopulateWithRandoms(r,game.IOInfo,_IsLazy);

            for (int generationCounter=0; generationCounter < _numGenerations; generationCounter++)
            {
                RunGeneration(game, trainableState,currentGeneration);

                if(showProgress && ((generationCounter % demonstrateEveryXIterations) == 0))
                {
                    game.Demonstrate(currentGeneration.GetBestSpecies(), trainableState);
                    trainableState.Reset();
                }
            }

            return currentGeneration.GetBestSpecies();
        }

        private void RunGeneration(IDiscreteGameManager game,IDiscreteGameState state,Generation currentGeneration)
        {
            currentGeneration.ScoreGeneration(game, state);
            currentGeneration.Kill(_numToKillPerGeneration);
            currentGeneration.Multiply();
        }
    }
}
