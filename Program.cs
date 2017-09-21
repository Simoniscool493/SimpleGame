
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.Games.FoodEatingGame;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 10000, 
                numToKill: 10, 
                numInGeneration: 30, 
                mutationRate: 0.1
            );

            var runner = new FoodEatingGameManager(timerLength: 20);
            var stateProvider = new SingleRandomFoodEatingGameStateProvider();

            var decider = genAlg.Train(runner, stateProvider, showProgress: true, demonstrateEveryXIterations: 2500);

            runner.Demonstrate(decider, stateProvider.GetStateForNextGeneration());
        }
    }
}
