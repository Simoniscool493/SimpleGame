using SimpleGame.AI.DecisionTrees;
using SimpleGame.DataPayloads;
using SimpleGame.GeneticAlgorithm;
using System;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var genAlg = new GeneticAlgorithmRunner(1000, 10, 30, 0.2);
            var runner = new FoodEatingGameRunner(20);

            var decider = genAlg.Train(runner);

            runner.Demonstrate(decider,FoodEatingGameBoard.GetRandomBoard());
        }
    }


}
