
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
using System.Collections;
using System.Linq;
using System.Diagnostics;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = SimpleGameLoggerManager.SetupLogger();
            logger.Debug("Simple Game Logger Created");

            //StandardTests(logger);
            //return;

            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 1000000,
                numToKill: 2,
                numInGeneration: 5,
                numOfTimesToTestASpecies: 1,
                mutationRate: -1,
                deciderType: DeciderType.LazyMatrix
            );

            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var tester = new SimpleGameTester();

            var decider = genAlg.Train(runner, stateProvider, showGameProgress: false, printBasicInfo: true, demonstrateEveryXIterations: 250);
            //var decider = DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\2390.dc");

            Console.WriteLine("Ready to demonstrate. Please press enter.");
            Console.ReadLine();

            decider.SaveToFile("C:\\ProjectLogs\\Simaneel.dc");

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

        public static void StandardTests(ILog logger)
        {
            Console.WriteLine("Ready to begin standard tests. Please enter comment for test.");
            var comment = Console.ReadLine();
            logger.Debug("Starting test with comment:\"" + comment + "\"");
            Console.WriteLine("Starting");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            var generationTestParameters = new GenAlgTestingStartParamaters
            (
                timesToTestEachConfiguration: 50,
                incrementToRecord: 101,
                numGenerationParamaters: BuildList(25,100),
                percentToKillParamaters: BuildList(10),
                generationSizeParamaters: BuildList(10,20),
                iterationsOfTestingPerSpeciesParamaters: BuildList(1),
                mutationPercentParamaters: BuildList(-1),
                deciderTypeParamaters: BuildList((int)DeciderType.LazyMatrix)
            );

            for (int i = 0; i < 3; i++)
            {
                var results = SimpleGameTester.TestGeneticAlgorithm(generationTestParameters, new PacmanManager(), new PacmanStateProvider(), scoreToReach: 0);
                results.PrintAndLogGenerationTest(logger);
            }

            var toScoreTestParameters = new GenAlgTestingStartParamaters
            (
                timesToTestEachConfiguration: 10,
                incrementToRecord: 101,
                numGenerationParamaters: BuildList(1),
                percentToKillParamaters: BuildList(10),
                generationSizeParamaters: BuildList(10,20),
                iterationsOfTestingPerSpeciesParamaters: BuildList(1),
                mutationPercentParamaters: BuildList(-1),
                deciderTypeParamaters: BuildList((int)DeciderType.LazyMatrix)
            );

            for (int i = 0; i < 2; i++)
            {
                var result1 = SimpleGameTester.TestGeneticAlgorithm(toScoreTestParameters, new PacmanManager(), new PacmanStateProvider(), scoreToReach: 500);
                result1.PrintAndLogScoreTest(logger);

                var result2 = SimpleGameTester.TestGeneticAlgorithm(toScoreTestParameters, new PacmanManager(), new PacmanStateProvider(), scoreToReach: 600);
                result2.PrintAndLogScoreTest(logger);
            }

            watch.Stop();
            logger.Debug("Total test time: " + watch.Elapsed);

            Console.WriteLine("Tests Finished");
            Console.ReadLine();
        }

        public static void RandomTesting()
        {
            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var decider = new RandomDiscreteDecider(new Random(), runner.IOInfo);
            var state = stateProvider.GetStateForNextGeneration();

            List<int> scores = new List<int>();

            for(int i=0;i<1000;i++)
            {
                scores.Add(runner.Score(decider, state));
                state.Reset();
            }

            var avg = (scores).Average();
        }
    }
}
