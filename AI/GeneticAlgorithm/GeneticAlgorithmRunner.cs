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

        public GeneticAlgorithmRunner(int numGenerations,int numToKill,int numInGeneration,double mutationRate)
        {
            _numGenerations = numGenerations;
            _numToKillPerGeneration = numToKill;
            _numInGeneration = numInGeneration;
            _mutationRate = mutationRate;
        }

        public IDiscreteDecider Train(IDiscreteGame game)
        {
            var trainableState = game.GetNextStateForTraining();
            var r = new Random();

            Generation currentGeneration = new Generation(_numInGeneration,_mutationRate);

            currentGeneration.PopulateWithRandoms(r,game.IOInfo);

            for (int j=0;j<_numGenerations;j++)
            {
                RunGeneration(game, trainableState,currentGeneration);
            }

            return currentGeneration.GetBestSpecies();
        }

        private void RunGeneration(IDiscreteGame game,IGameState state,Generation currentGeneration)
        {
            currentGeneration.ScoreGeneration(game, state);
            currentGeneration.Kill(_numToKillPerGeneration);
            currentGeneration.Multiply();
        }
    }
}
