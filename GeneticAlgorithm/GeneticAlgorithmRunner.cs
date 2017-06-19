using SimpleGame.Deciders;
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
        private bool _shouldILog;
        private int _numGenerations;
        private int _numToKillPerGeneration;
        private int _numInGeneration;
        private double _mutationRate;

        public GeneticAlgorithmRunner(bool shouldILog,int numGenerations,int numToKill,int numInGeneration,double mutationRate)
        {
            _shouldILog = shouldILog;
            _numGenerations = numGenerations;
            _numToKillPerGeneration = numToKill;
            _numInGeneration = numInGeneration;
            _mutationRate = mutationRate;
        }

        public Generation Train(GameBoard g)
        {
            Generation currentGeneration = new Generation(_numInGeneration,_mutationRate);

            for(int i=0;i<_numInGeneration;i++)
            {
                var randomMatrix = DecisionMatrix.GetRandomMatrix();
                currentGeneration.Add(randomMatrix);
            }

            for (int j=0;j<_numGenerations;j++)
            {
                RunGeneration(g,currentGeneration);
            }

            return currentGeneration;
        }

        private void RunGeneration(GameBoard g,Generation currentGeneration)
        {
            var scores = new Dictionary<DecisionMatrix, int>();
            var runner = new GameRunner(false,false,10);

            currentGeneration.ScoreGeneration(runner,g);
            currentGeneration.Kill(_numToKillPerGeneration);
            currentGeneration.Multiply();
        }
    }
}
