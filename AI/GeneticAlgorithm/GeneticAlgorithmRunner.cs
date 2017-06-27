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
            var trainableState = g.GetRandomTrainableState();
            var r = new Random();

            Generation currentGeneration = new Generation(_numInGeneration,_mutationRate);

            for(int i=0;i<_numInGeneration;i++)
            {
                var randomMatrix = DecisionMatrix.GetRandomIOMapping(_inputInfo,_outputInfo);
                currentGeneration.Add(randomMatrix);
            }

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
