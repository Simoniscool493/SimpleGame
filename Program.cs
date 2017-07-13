
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.Games.FoodEatingGame;

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
