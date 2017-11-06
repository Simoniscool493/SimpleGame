
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
        public const string LogsPath = "C:\\ProjectLogs\\";

        static void Main(string[] args)
        {
            //Testing();

            var logger = SimpleGameLoggerManager.SetupLogger();
            logger.Debug("Simple Game Logger Created");

            //StandardTests(logger);
            //return;

            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;


            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 100000,
                numToKill: 10,
                numInGeneration: 30,
                numOfTimesToTestASpecies: 1,
                mutationRate: -1,
                deciderType: DiscreteDeciderType.LazyMatrix
            );

            var tester = new SimpleGameTester();

            var decider = genAlg.Train(runner, stateProvider, showGameProgress: true, printBasicInfo: false, demonstrateEveryXIterations: 1000);
            decider.SaveToFile($"C:\\ProjectLogs\\ADecider.dc");
            //var decider = DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\2810.dc");
            //var state = stateProvider.GetStateForNextGeneration();
            //var score = runner.Score(decider, state);

            //Console.WriteLine();

            Console.WriteLine("Ready to demonstrate. Please press enter.");
            Console.ReadLine();

            //decider.SaveToFile($"C:\\ProjectLogs\\No_Ghosts_1560.dc");


            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(decider, state);

            var scores = new List<int>();
            int best = 0;
            double avg = 0;
            int worst = 0;

            for(int i=0;i<1;i++)
            {
                //var score = runner.Score(decider, stateProvider.GetStateForNextGeneration());
                //scores.Add(score);
                avg = scores.Average();
                best = scores.Max();
                worst = scores.Min();
            }


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
                mutationPercentParamaters: BuildList(5,10),
                deciderTypeParamaters: BuildList((int)DiscreteDeciderType.LazyMatrix)
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
                deciderTypeParamaters: BuildList((int)DiscreteDeciderType.LazyMatrix)
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

        static void Testing()
        {
            var list0 = new List<Tuple<int, int>>() { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 3) };
            var list1 = new List<Tuple<int, int>>() { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 3) };
            var list2 = new List<Tuple<int, int>>() { new Tuple<int, int>(1, 3), new Tuple<int, int>(1, 2) };
            var list3 = new List<Tuple<int, int>>() { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 4) };

            var _0and1 = list0.SequenceEqual(list1);
            var _1and1 = list1.SequenceEqual(list1);
            var _1and2 = ScrambledEquals(list1,list2);
            var _1and3 = list1.SequenceEqual(list3);
            var _2and3 = list2.SequenceEqual(list3);

            var info = new PacmanManager().IOInfo;

            var heuristic1 = new Heuristic(0, info);
            heuristic1.Conditions.Add(new Tuple<int, int>(0, 10));
            heuristic1.Conditions.Add(new Tuple<int, int>(1, 9));
            heuristic1.Conditions.Add(new Tuple<int, int>(2, 8));
            heuristic1.Conditions.Add(new Tuple<int, int>(3, 7));

            var heuristic2 = new Heuristic(1, info);
            heuristic2.Conditions.Add(new Tuple<int, int>(4, 10));
            heuristic2.Conditions.Add(new Tuple<int, int>(5, 9));
            heuristic2.Conditions.Add(new Tuple<int, int>(6, 8));
            heuristic2.Conditions.Add(new Tuple<int, int>(7, 7));

            var heuristic3 = new Heuristic(2, info);
            heuristic3.Conditions.Add(new Tuple<int, int>(1, 10));
            heuristic3.Conditions.Add(new Tuple<int, int>(2, 9));
            heuristic3.Conditions.Add(new Tuple<int, int>(3, 8));
            heuristic3.Conditions.Add(new Tuple<int, int>(4, 7));

            var heuristic4 = new Heuristic(3, info);
            heuristic4.Conditions.Add(new Tuple<int, int>(2, 10));
            heuristic4.Conditions.Add(new Tuple<int, int>(3, 9));
            heuristic4.Conditions.Add(new Tuple<int, int>(4, 8));
            heuristic4.Conditions.Add(new Tuple<int, int>(5, 7));

            var heuristicDecider1 = new HeuristicBuildingDecider(new Random(1), info);
            var heuristicDecider2 = new HeuristicBuildingDecider(new Random(1), info);

            var species1 = new GeneticAlgorithmSpecies(heuristicDecider1);
            var species2 = new GeneticAlgorithmSpecies(heuristicDecider2);

            heuristicDecider1.Heuristics.Add(heuristic1);
            heuristicDecider1.Heuristics.Add(heuristic2);
            heuristicDecider2.Heuristics.Add(heuristic3);
            heuristicDecider2.Heuristics.Add(heuristic4);

            var heuristicDecider3 = species1.Cross(species2, 0.1, new Random(1));


        }

        public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2)
        {
            var cnt = new Dictionary<T, int>();
            foreach (T s in list1)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]++;
                }
                else
                {
                    cnt.Add(s, 1);
                }
            }
            foreach (T s in list2)
            {
                if (cnt.ContainsKey(s))
                {
                    cnt[s]--;
                }
                else
                {
                    return false;
                }
            }
            return cnt.Values.All(c => c == 0);
        }
    }
}
