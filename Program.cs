
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Games;
using SimpleGame.Games.FoodEatingGame;
using SimpleGame.Games.SimplePacman;
using System;
using System.Collections.Generic;
using log4net;
using Pacman;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = SimpleGameLoggerManager.SetupLogger();
            logger.Debug("Simple Game Logger Created");

            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 1000,
                numToKill: 15,
                numInGeneration: 30,
                numOfTimesToTestASpecies: 1,
                mutationRate: 0.1,
                deciderType: DeciderType.LazyMatrix
            );


            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;

            //runner.Demonstrate(new SimpleGame.Deciders.Discrete.RandomDiscreteDecider(new Random(), runner.IOInfo), stateProvider.GetStateForDemonstration());
            //Console.ReadLine();
            //return;

            var decider = genAlg.Train(runner, stateProvider, showProgress: false, demonstrateEveryXIterations: 0);

            ActualPacmanGameInstance.isLogging = true;

            runner.Demonstrate(decider, stateProvider.GetStateForDemonstration());

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }
    }
}
