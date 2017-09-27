
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.Games.FoodEatingGame;
using SimpleGame.Games.SimplePacman;
using System;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 10, 
                numToKill: 2, 
                numInGeneration: 5, 
                mutationRate: 0.1,
                isLazy: true
            );

            var runner = new PacmanManager();
            var stateProvider = runner.StateProvider;

            var decider = genAlg.Train(runner, stateProvider, showProgress: true, demonstrateEveryXIterations: 2500);

            runner.Demonstrate(decider, stateProvider.GetStateForNextGeneration());
        }
    }
}
