
using SimpleGame.Deciders;
using SimpleGame.Games.SimplePacman;
using System;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Deciders.HeuristicBuilder;
using SimpleGame.AI;
using SimpleGame.Games.SpaceInvaders;
using System.Collections.Generic;
using System.Threading;
using SimpleGame.Games.Snake;
using log4net;
using SimpleGame.Games.FoodEatingGame;

namespace SimpleGame
{
    class Program
    {
        public const string LogsPath = "C:\\ProjectLogs\\";

        static void Main(string[] args)
        {
            CSVWriter.WriteSinglePathMutationRunnerCSV(@"C:\ProjectLogs\SimpleGameLog_3-4-2018.txt", @"C:\ProjectLogs\Pacman100Games.csv");
            return;
            var logger = SimpleGameLoggerManager.SetupLogger();
            PacmanTests(logger);

            var runner = new SnakeManager();
            var state = runner.StateProvider.GetStateForDemonstration();
            var decider = new RandomDiscreteDecider(new Random(), runner.IOInfo);

            runner.Demonstrate(decider, state);

            List<int> scores = new List<int>();

            for(int i=0;i<10;i++)
            {
                var score = runner.Score(decider, state);

                scores.Add(score);
            }

            PacmanTests(logger);

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }

        public static void PacmanTests(ILog logger)
        {
            var r = new Random();
            var runner = new PacmanManager(logger);                 //Choose which game

            //var dec = DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\testSave.sg");
            //var loadstate = runner.StateProvider.GetStateForDemonstration();
            //runner.DemonstrateWithLogging(dec, loadstate);

            var learner = new SinglePathMutationRunner(logger,runner, runner.StateProvider, true, true, true);     //Choose learning method
            learner.BestSpecies = new DeciderSpecies(new HeuristicBuildingDecider(r, runner.IOInfo));       //Create decider

            learner.GenerationSize = 5;             //25
            learner.MaxHeuristicsToTake = 10;       //10
            learner.TimesToTestPerSpecies = 50;     //100
            learner.MinimizeComplexity = true;
            learner.IncludePreviousBestWhenIteratingForwards = true;

            learner.Optimize(50, 0.1, r);

            SpaceInvadersManager.ShouldLog = true;

            var score = runner.Score(learner.BestSpecies, runner.StateProvider.GetStateForNextGeneration());
            //learner.BestSpecies.SaveToFile("C:\\ProjectLogs\\testSave.sg");
            
            var state = runner.StateProvider.GetStateForDemonstration();
            runner.Demonstrate(learner.BestSpecies, state);
        }
    }
}
