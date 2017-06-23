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
        private int _timerLength;

        public GeneticAlgorithmRunner(bool shouldILog,int numGenerations,int numToKill,int numInGeneration,double mutationRate,int timerLength)
        {
            _shouldILog = shouldILog;
            _numGenerations = numGenerations;
            _numToKillPerGeneration = numToKill;
            _numInGeneration = numInGeneration;
            _mutationRate = mutationRate;
            _timerLength = timerLength;
        }

        public Generation Train(FoodEatingGameBoard g)
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
                currentGeneration.RunSample(g,_timerLength);
            }

            return currentGeneration;
        }

        private void RunGeneration(FoodEatingGameBoard g,Generation currentGeneration)
        {
            var runner = new FoodEatingGameRunner(false,false,_timerLength);

            currentGeneration.ScoreGeneration(runner,g);
            currentGeneration.Kill(_numToKillPerGeneration);
            currentGeneration.Multiply();
        }
    }
}
