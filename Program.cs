
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
using SimpleGame.Deciders.HeuristicBuilder;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var runner = new PacmanManager();

            var randomDecider = new RandomDiscreteDecider(new Random(), runner.IOInfo);
            var randomAvg = SuccessTesting(randomDecider, 100);
            var heuristicDecidersByScore = new Dictionary<HeuristicBuildingDecider, double>();
            var r = new Random();

            for(int i=0;i<1000;i++)
            {
                var heuristicDecider = new HeuristicBuildingDecider(r, runner.IOInfo);
                heuristicDecider.AddRandomHeuristics(5);
                var heuristicAvg = SuccessTesting(heuristicDecider, 1);

                heuristicDecidersByScore[heuristicDecider] = heuristicAvg;
            }

            var best = heuristicDecidersByScore.OrderBy(kv=>kv.Value).Reverse().First();

            for(int i=0;i<100000;i++)
            {
                heuristicDecidersByScore.Clear();
                heuristicDecidersByScore[best.Key] = best.Value;

                for (int j=0;j<10;j++)
                {
                    var heuristicDecider = best.Key.GetSingleMutated().GetSingleMutated();
                    var heuristicAvg = SuccessTesting(heuristicDecider, 1);
                    heuristicDecidersByScore[heuristicDecider] = heuristicAvg;
                }
                



                best = heuristicDecidersByScore.OrderBy(kv => kv.Value).Reverse().First();
                Console.WriteLine("Score: " + best.Value + " Genes: " + best.Key.NumGenes);
            }

            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(best.Key, state);


            Console.WriteLine();




            /*var logger = SimpleGameLoggerManager.SetupLogger();
            logger.Debug("Simple Game Logger Created");

            //StandardTests(logger);
            //return;

            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 1000,
                numToKill: 80,
                numInGeneration: 100,
                numOfTimesToTestASpecies: 1,
                mutationRate: -1,
                deciderType: DeciderType.LazyMatrix
            );

            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var tester = new SimpleGameTester();

            //var decider = genAlg.Train(runner, stateProvider, showGameProgress: false, printBasicInfo: true, demonstrateEveryXIterations: 250);
            var decider = DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\No_Ghosts_1560.dc");

            //Console.WriteLine();
            decider.SaveToFile($"C:\\ProjectLogs\\No_Ghosts_1560.dc");

            Console.WriteLine("Ready to demonstrate. Please press enter.");
            Console.ReadLine();


            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(decider, state);

            var scores = new List<int>();
            int best = 0;
            double avg = 0;
            int worst = 0;

            for(int i=0;i<1;i++)
            {
                var score = runner.Score(decider, stateProvider.GetStateForNextGeneration());
                scores.Add(score);
                avg = scores.Average();
                best = scores.Max();
                worst = scores.Min();


            }


            return;




            //ActualPacmanGameInstance.isLogging = true;

            //state.Dispose();


            Console.WriteLine("Finished.");
            Console.ReadLine();*/
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
                mutationPercentParamaters: BuildList(5,10),
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
                mutationPercentParamaters: BuildList(5,10),
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

        public static double SuccessTesting(IDiscreteDecider decider,int numTimes)
        {
            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var state = stateProvider.GetStateForNextGeneration();

            List<int> scores = new List<int>();

            for(int i=0;i< numTimes; i++)
            {
                scores.Add(runner.Score(decider, state));
                state.Reset();
            }

            return (scores).Average();
        }
    }
}
