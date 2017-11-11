
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

            var sets = Directory.GetFiles("C:\\ProjectLogs\\GoodHeuristicSets\\").Where(f=>f.Contains(".dc"));
            var deciders = new List<DeciderSpecies>();

            foreach(var file in sets)
            {
                var decider = (DeciderSpecies)DiscreteDeciderLoader.LoadFromFile(file);
                decider.Score = (int)SimpleGameTester.SuccessTesting(decider, 20);
                deciders.Add(decider);
            }

            return;

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }





        public static void Testing2()
        {
            var r = new Random();
            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;
            var learner = new SinglePathMutationRunner(runner, stateProvider);
            learner.CurrentBest = new DeciderSpecies(new HeuristicBuildingDecider(r, runner.IOInfo));

            learner.GenerationSize = 25;
            learner.MaxConditionsToTake = 20;
            learner.MaxHeuristicsToTake = 10;
            learner.TimesToTestPerSpecies = 1;

            learner.Optimize(30, r);

            learner.CurrentBest.SaveToFile("C:\\ProjectLogs\\GoodHeuristicSets\\" + learner.CurrentBest.Score + "_Survive.dc");

            ActualPacmanGameInstance.TIMER_TICK_PENALTY = 0;
            ActualPacmanGameInstance.FOOD_SCORE = 10;
            ActualPacmanGameInstance.SUPER_FOOD_SCORE = 50;
            ActualPacmanGameInstance.GHOST_EATING_SCORE = 50;

            var score = runner.Score(learner.CurrentBest, stateProvider.GetStateForNextGeneration());

            var state = stateProvider.GetStateForDemonstration();
            runner.Demonstrate(learner.CurrentBest, state);
        }
    }
}
