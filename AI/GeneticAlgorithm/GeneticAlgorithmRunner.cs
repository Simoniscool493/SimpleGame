using SimpleGame.AI;
using SimpleGame.DataPayloads;
using SimpleGame.Deciders;
using SimpleGame.Games;
using SimpleGame.Games.FoodEatingGame;
using SimpleGame.Players;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleGame.GeneticAlgorithm
{
    class GeneticAlgorithmRunner : IDiscreteDecisionModel
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

        public IDiscreteDecider Train(IDiscreteGame g)
        {
            var trainableState = g.GetNextStateForTraining();
            var r = new Random();

            Generation currentGeneration = new Generation(g.IOInfo,_numInGeneration,_mutationRate);

            currentGeneration.PopulateWithRandoms(r);

            for (int j=0;j<_numGenerations;j++)
            {
                RunGeneration(g,trainableState,currentGeneration);
            }

            return currentGeneration.GetBestSpecies();
        }

        private void RunGeneration(IDiscreteGame g,IGameState s,Generation currentGeneration)
        {
            currentGeneration.ScoreGeneration(g, s);
            currentGeneration.Kill(_numToKillPerGeneration);
            currentGeneration.Multiply();
        }
    }
}
