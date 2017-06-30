using SimpleGame.AI.DecisionTrees;
using SimpleGame.DataPayloads;
using SimpleGame.DataPayloads.DiscreteData.DecisionMatrix;
using SimpleGame.GeneticAlgorithm;
using System;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var genAlg = new GeneticAlgorithmRunner(10000, 10, 30, 0.1);
            var runner = new FoodEatingGameRunner(100);

            var deider = genAlg.Train(runner);

            runner.Demonstrate(deider, FoodEatingGameBoard.GetRandomBoard());
        }
    }


}
