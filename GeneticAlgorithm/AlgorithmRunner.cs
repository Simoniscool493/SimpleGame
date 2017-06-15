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
    class AlgorithmRunner
    {
        private bool _shouldILog;
        private int _numGenerations;
        private int _numToKillPerGeneration;
        private int _numInGeneration;

        public AlgorithmRunner(bool shouldILog,int numGenerations,int numToKill,int numInGeneration)
        {
            _shouldILog = shouldILog;
            _numGenerations = numGenerations;
            _numToKillPerGeneration = numToKill;
            _numInGeneration = numInGeneration;
        }

        public List<DecisionMatrix> Train(GameBoard g)
        {
            List<DecisionMatrix> currentGeneration = new List<DecisionMatrix>();

            for(int i=0;i<_numInGeneration;i++)
            {
                var randomMatrix = DecisionMatrix.GetRandomMatrix();
                Console.WriteLine(randomMatrix.Decide(new[]{(ItemAtPoint)0, (ItemAtPoint)0, (ItemAtPoint)0, (ItemAtPoint)0 }));
                Thread.Sleep(10);
                currentGeneration.Add(randomMatrix);
            }

            for (int j=0;j<_numGenerations;j++)
            {
                currentGeneration = RunGeneration(g,currentGeneration);
            }

            return currentGeneration;
        }

        private List<DecisionMatrix> RunGeneration(GameBoard g,List<DecisionMatrix> previousGeneration)
        {
            var nextGeneration = new List<DecisionMatrix>();
            var scores = new Dictionary<DecisionMatrix, int>();
            var runner = new GameRunner(false,false,10);

            foreach(var matrix in previousGeneration)
            {
                var score = runner.RunPlayerOnBoard(new AiGridPlayer(new MatrixDecider(matrix)), g,0,0);
                scores[matrix] = score;
            }

            return nextGeneration;
        }
    }
}
