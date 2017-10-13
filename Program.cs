
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
using SimpleGame.Deciders.Discrete;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = SimpleGameLoggerManager.SetupLogger();
            logger.Debug("Simple Game Logger Created");

            /*var paramaters = new GenAlgTestingStartParamaters
            (
                timesToTestEachConfiguration:50,
                incrementToRecord:101,
                numGenerationParamaters:BuildList(70),
                percentToKillParamaters:BuildList(10),
                generationSizeParamaters:BuildList(10, 20, 30),
                iterationsOfTestingPerSpeciesParamaters:BuildList(1),
                mutationPercentParamaters:BuildList(10),
                deciderTypeParamaters:BuildList((int)DeciderType.LazyMatrix)
            );

            for(int i=0;i<4; i++)
            {
                var results = SimpleGameTester.TestGeneticAlgorithm(paramaters, new PacmanManager(), new PacmanStateProvider());
                results.PrintAndLog(logger);
            }

            Console.WriteLine("Done.");
            Console.ReadLine();
            return;*/






            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 1000,
                numToKill: 1,
                numInGeneration: 10,
                numOfTimesToTestASpecies: 1,
                mutationRate: 0.1,
                deciderType: DeciderType.LazyMatrix
            );


            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var tester = new SimpleGameTester();


            /*Console.WriteLine("Ready to begin. Please press enter.");
            Console.ReadLine();
            Console.WriteLine("Starting");*/

            //var decider = genAlg.Train(runner, stateProvider, showGameProgress: false, printBasicInfo: false, demonstrateEveryXIterations: 250);

            var decider = DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\test.dc");

            /*Console.WriteLine("Ready to demonstrate. Please press enter.");
            Console.ReadLine();*/

            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(decider, state);

            decider.SaveToFile("C:\\ProjectLogs\\test.dc");



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
