
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
using SimpleGame.AI;
using System.IO;
using SimpleGame.Games.Iris;

namespace SimpleGame
{
    class Program
    {
        public const string LogsPath = "C:\\ProjectLogs\\";

        static void Main(string[] args)
        {
            Testing2();
            return;

            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;


            var decider = DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\GoodHeuristicSets\\182_EatPellets_GeneralSolution.dc");
            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(decider, state);


            var sets = Directory.GetFiles("C:\\ProjectLogs\\GoodHeuristicSets\\").Where(f=>f.Contains(".dc"));
            var deciders = new Dictionary<int,string>();

            foreach(var file in sets)
            {
                var decidert = (DeciderSpecies)DiscreteDeciderLoader.LoadFromFile(file);

                decidert.Score = (int)SimpleGameTester.SuccessTesting(runner,decider, 200);
                deciders[decidert.Score] = file;
            }

            var sorted = deciders.OrderBy(k => k.Key).Reverse().ToArray();


            return;

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }


        public static List<Tuple<int[],int>> tested = new List<Tuple<int[], int>>();
        public static List<Tuple<int[], int>> demoed = new List<Tuple<int[], int>>();


        public static void Testing2()
        {
            var r = new Random();
            var runner = new PacmanManager();
            var stateProvider = runner.StateProvider;
            var learner = new SinglePathMutationRunner(runner, stateProvider);
            learner.CurrentBest = new DeciderSpecies(new HeuristicBuildingDecider(r, runner.IOInfo));
            //learner.CurrentBest = (DeciderSpecies)DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\GoodHeuristicSets\\2800.dc");

            learner.GenerationSize = 25; //25
            //learner.MaxConditionsToTake = 20; //20
            learner.MaxHeuristicsToTake = 10; //10
            learner.TimesToTestPerSpecies = 1; //100
            learner.MinimizeComplexity = true;
            learner.IncludePreviousBestWhenIteratingForwards = true;

            learner.Optimize(5, r);

            //var score2 = SimpleGameTester.SetRandomSuccessTesting(runner, learner.CurrentBest, 100);

            //learner.CurrentBest.RandomSeed = 1;
            //learner.CurrentBest.SaveToFile("C:\\ProjectLogs\\GoodHeuristicSets\\" + learner.CurrentBest.Score + ".dc");

            ActualPacmanGameInstance.TIMER_TICK_PENALTY = 0;
            ActualPacmanGameInstance.FOOD_SCORE = 10;
            ActualPacmanGameInstance.SUPER_FOOD_SCORE = 50;
            ActualPacmanGameInstance.GHOST_EATING_SCORE = 50;
            ActualPacmanGameInstance.GHOST_SEEING_SCORE = 0;
            ActualPacmanGameInstance.DEATH_PENALTY = 0;
            ActualPacmanGameInstance.WALL_BUMPING_PENALTY = 0;
            ActualPacmanGameInstance.OLD_POSITION_PENALTY = 0;
            ActualPacmanGameInstance.NEW_POSITION_SCORE = 0;


            //var score3 = SimpleGameTester.SetRandomSuccessTesting(runner, learner.CurrentBest, 100);
            var score = runner.Score(learner.CurrentBest, stateProvider.GetStateForNextGeneration());

            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(learner.CurrentBest, state);
        }
    }
}
