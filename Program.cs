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
            var genAlg = new GeneticAlgorithmRunner(5000, 10, 30, 0.2);
            var runner = new FoodEatingGameRunner(40);

            var deider = genAlg.Train(runner);

            runner.Demonstrate(deider, FoodEatingGameBoard.GetRandomBoard());
        }
    }


}
