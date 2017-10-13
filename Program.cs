
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
using SimpleGame.Metrics;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = SimpleGameLoggerManager.SetupLogger();
            logger.Debug("Simple Game Logger Created");

            var paramaters = new GenAlgTestingStartParamaters
            (
                timesToTestEachConfiguration:10,
                incrementToRecord:51,
                numGenerationParamaters:BuildList(10),
                percentToKillParamaters:BuildList(10,20),
                generationSizeParamaters:BuildList(10),
                iterationsOfTestingPerSpeciesParamaters:BuildList(1),
                mutationPercentParamaters:BuildList(10),
                deciderTypeParamaters:BuildList((int)DeciderType.LazyMatrix)
            );

            for(int i=0;i<3; i++)
            {
                var results = SimpleGameTester.TestGeneticAlgorithm(paramaters, new PacmanManager(), new PacmanStateProvider());
                results.PrintAndLog(logger);
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
            return;






            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 5000,
                numToKill: 1,
                numInGeneration: 13,
                numOfTimesToTestASpecies: 1,
                mutationRate: 0.1,
                deciderType: DeciderType.LazyMatrix
            );


            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var tester = new SimpleGameTester();


            Console.WriteLine("Ready to begin. Please press enter.");
            Console.ReadLine();

            var decider = genAlg.Train(runner, stateProvider, showGameProgress: false, printBasicInfo: true, demonstrateEveryXIterations: 250);

            Console.WriteLine("Ready to demonstrate. Please press enter.");
            Console.ReadLine();

            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(decider, state);
            return;




            //ActualPacmanGameInstance.isLogging = true;

            //state.Dispose();


            Console.WriteLine("Finished.");
            Console.ReadLine();
        }

        public static List<int> BuildList(params int[] p)
        {
            var list = new List<int>();

            foreach(int i in p)
            {
                list.Add(i);
            }

            return list;
        }
    }
}
