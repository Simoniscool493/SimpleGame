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
            var genAlg = new GeneticAlgorithmRunner(false,1000,5,30,0.1);
            var board = new GameBoard();

            var final = genAlg.Train(board);

            Console.ReadLine();
        }
    }


}
