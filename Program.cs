using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Neuro;
using Accord.Statistics.Kernels;
using SimpleGame.GeneticAlgorithm;
using System;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var genAlg = new AlgorithmRunner(false,10,2,30);
            var board = new GameBoard();

            genAlg.Train(board);

            Console.ReadLine();
        }
    }


}
