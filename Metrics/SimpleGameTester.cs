using log4net;
using Pacman;
using SimpleGame.AI;
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.Deciders;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games;
using SimpleGame.Games.SimplePacman;
using SimpleGame.Metrics.GenAlg.Results;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics
{
    class SimpleGameTester
    {
        public static GenAlgTestingOverallResults TestGeneticAlgorithm(GenAlgTestingStartParamaters parameterList, IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider, int scoreToReach)
        {
            var output = new GenAlgTestingOverallResults(parameterList,scoreToReach);
            var watch = new Stopwatch();
            watch.Start();

            var allPermutations = parameterList.GetAllPermutations();

            foreach (var perm in allPermutations)
            {
                output[perm] = TestSingleSetOfParamaters(parameterList.TimesToTestEachConfiguration, parameterList.IncrementToRecord, perm, runner, stateProvider,scoreToReach);
            }

            watch.Stop();
            output.totalTimeTaken = watch.Elapsed;

            return output;
        }

        private static GenAlgTestResults TestSingleSetOfParamaters(int timesToTest,int incrementToRecord,int[] paramaters, IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider,int scoreToReach)
        {
            GenAlgTestResults tests;

            if(scoreToReach>0)
            {
                tests = new GenAlgToScoreTestResults();
            }
            else
            {
                tests = new GenAlgSpecifiedGenerationNumTestResults();
            }

            var numGenerationsParamater = (scoreToReach>0) ? int.MaxValue : paramaters[0];
            var percentToKillParamater = paramaters[1];
            var generationSizeParamater = paramaters[2];
            var iterationsOfTestingPerSpeciesParamater = paramaters[3];
            var mutationPercentParamater = paramaters[4];
            var deciderTypeParamater = (DiscreteDeciderType)paramaters[5];

            int numToKillParamater = (int)Math.Floor(((percentToKillParamater / (double)100) * generationSizeParamater));
            double mutationRateParamater = mutationPercentParamater / (double)100;

            if(numToKillParamater<1)
            {
                throw new Exception("No species are being killed.");
            }

            var genAlg = new EvaluationGeneticAlgorithmRunner
            (
                numGenerations: numGenerationsParamater,
                numToKill: numToKillParamater,
                numInGeneration: generationSizeParamater,
                numOfTimesToTestASpecies: iterationsOfTestingPerSpeciesParamater,
                mutationRate: mutationRateParamater,
                deciderType: deciderTypeParamater,
                recorder: null
            );

            for (int i = 0; i < timesToTest; i++)
            {
                var recorder = RunSingleGeneticAlgorithmTrainingSession(runner, stateProvider, genAlg, incrementToRecord,scoreToReach);
                tests.Add(recorder);
            }

            return tests;
        }

        private static GenAlgTrainingSessionRecorder RunSingleGeneticAlgorithmTrainingSession(IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider, EvaluationGeneticAlgorithmRunner genAlg,int incrementToTestWith,int scoreToReach)
        {
            GenAlgTrainingSessionRecorder recorder = new GenAlgTrainingSessionRecorder(incrementToTestWith, scoreToReach);

            genAlg.Recorder = recorder;

            recorder.TotalTimer.Start();
            genAlg.Train(runner, stateProvider, false,false, 0);
            recorder.TotalTimer.Stop();

            recorder.TotalTimeTaken = recorder.TotalTimer.Elapsed;

            return recorder;
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
                numGenerationParamaters: BuildList(25, 100),
                percentToKillParamaters: BuildList(10),
                generationSizeParamaters: BuildList(10, 20),
                iterationsOfTestingPerSpeciesParamaters: BuildList(1),
                mutationPercentParamaters: BuildList(5, 10),
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
                generationSizeParamaters: BuildList(10, 20),
                iterationsOfTestingPerSpeciesParamaters: BuildList(1),
                mutationPercentParamaters: BuildList(5, 10),
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


        public static List<int> BuildList(params int[] p)
        {
            var list = new List<int>();

            foreach (int i in p)
            {
                list.Add(i);
            }

            return list;
        }

        public static double SuccessTesting(IDiscreteGameManager runner,IDiscreteDecider decider, int numTimes)
        {
            var stateProvider = runner.StateProvider;
            var state = stateProvider.GetStateForNextGeneration();

            List<int> scores = new List<int>();

            for (int i = 0; i < numTimes; i++)
            {
                scores.Add(runner.Score(decider, state));
                state.Reset();
            }

            scores.Sort();
            scores.Reverse();

            return (scores).Average();
        }

        public static double SetRandomSuccessTesting(IDiscreteGameManager runner,IDiscreteDecider decider, int numTimes)
        {
            var old = ActualPacmanGameInstance.RANDOM_SEED;
            var stateProvider = runner.StateProvider;

            List<int> scores = new List<int>();

            for (int i = 0; i < numTimes; i++)
            {
                //ActualPacmanGameInstance.RANDOM_SEED = i;
                var state = stateProvider.GetStateForNextGeneration();

                ActualPacmanGameInstance.RANDOM_SEED = i;
                scores.Add(runner.Score(decider, state));
                //state.Reset();
            }

            ActualPacmanGameInstance.RANDOM_SEED = old;
            return (scores).Average();
        }


        public static void Testing()
        {
            /*var list0 = new List<Tuple<int, int>>() { new Tuple<int, int>(1, 2), new Tuple<int, int>(1, 3) };
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

            var heuristicDecider3 = species1.Cross(species2, 0.1, new Random(1));*/


        }
    }
}
