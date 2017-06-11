using Accord.MachineLearning.DecisionTrees.Learning;
using Accord.MachineLearning.VectorMachines.Learning;
using Accord.Neuro;
using Accord.Statistics.Kernels;
using System;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            int[][] inputs =
            {
                new int[] { 100, 0, 0 },
                new int[] { 0, 100, 0 }, 
                new int[] { 0, 0, 100 }, 
                new int[] { 50, 50, 50 },
            };

            int[] outputs =
            { 
                1,2,3,4,
            };

            ID3Learning teacher = new ID3Learning();
            var tree = teacher.Learn(inputs, outputs);

            int[][] newInputs =
{
                new int[] { 90, 10, 5 },
                new int[] { 10, 90, 5 },
                new int[] { 10, 5, 90 },
                new int[] { 0, 0, 0 },
            };

            int[] predicted = tree.Decide(newInputs); 



            Console.ReadKey();


            return;

            var board = new GameBoard();
            var player = new GameRunner(false,true, 1, 1, 100);
            player.RunPlayerOnBoard(new ManualGridPlayer(), board);

            Console.ReadLine();
        }
    }


}
