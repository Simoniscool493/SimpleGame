using SimpleGame.GeneticAlgorithm;
using System;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var genAlg = new GeneticAlgorithmRunner(false,1000,5,10,0.1,20);
            var board = new FoodEatingGameBoard();

            var final = genAlg.Train(board);

            Console.ReadLine();
        }
    }


}
