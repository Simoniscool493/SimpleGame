using SimpleGame.GeneticAlgorithm;
using System;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var genAlg = new GeneticAlgorithmRunner(false,100000,1,5,0.08,50);
            var board = new FoodEatingGameBoard();

            Console.CursorVisible = false;

            var final = genAlg.Train(board);

            new FoodEatingGameRunner(false, true, 50).RunPlayerOnBoard(new ManualGridPlayer(), board, 0, 0);

            Console.ReadLine();
        }
    }


}
