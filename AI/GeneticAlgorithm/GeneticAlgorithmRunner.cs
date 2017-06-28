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
    class GeneticAlgorithmRunner
    {
        private DiscreteDataPayloadInfo _inputInfo;
        private DiscreteDataPayloadInfo _outputInfo;

        private int _numGenerations;
        private int _numToKillPerGeneration;
        private int _numInGeneration;
        private double _mutationRate;

        public GeneticAlgorithmRunner(DiscreteDataPayloadInfo inputInfo,DiscreteDataPayloadInfo outputInfo,int numGenerations,int numToKill,int numInGeneration,double mutationRate)
        {
            _inputInfo = inputInfo;
            _outputInfo = outputInfo;

            _numGenerations = numGenerations;
            _numToKillPerGeneration = numToKill;
            _numInGeneration = numInGeneration;
            _mutationRate = mutationRate;
        }

        public Generation Train(IDiscreteGame g)
        {
            var trainableState = g.GetState();
            var r = new Random();

            Generation currentGeneration = new Generation(_inputInfo,_outputInfo,_numInGeneration,_mutationRate);

            currentGeneration.PopulateWithRandoms();

            for (int j=0;j<_numGenerations;j++)
            {
                RunGeneration(g,trainableState,currentGeneration);
            }

            return currentGeneration;
        }

        private void RunGeneration(IDiscreteGame g,IGameState s,Generation currentGeneration)
        {
            currentGeneration.ScoreGeneration(g, s);
            currentGeneration.Kill(_numToKillPerGeneration);
            currentGeneration.Multiply();
        }
    }
}
