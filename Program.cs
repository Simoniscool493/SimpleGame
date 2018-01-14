
using SimpleGame.Deciders;
using SimpleGame.Games.SimplePacman;
using System;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Deciders.HeuristicBuilder;
using SimpleGame.AI;
using SimpleGame.Games.SpaceInvaders;

namespace SimpleGame
{
    class Program
    {
        public const string LogsPath = "C:\\ProjectLogs\\";

        static void Main(string[] args)
        {
            PacmanTests();

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }

        public static void PacmanTests()
        {
            var r = new Random();

            var runner = new SpaceInvadersManager();                                                                   //Choose which game
            var learner = new SinglePathMutationRunner(runner, runner.StateProvider,true,true);                 //Choose learning method
            learner.BestSpecies = new DeciderSpecies(new HeuristicBuildingDecider(r, runner.IOInfo));           //Create decider

            learner.GenerationSize = 25;        //25
            learner.MaxHeuristicsToTake = 10;   //10
            learner.TimesToTestPerSpecies = 1;  //100
            learner.MinimizeComplexity = true;
            learner.IncludePreviousBestWhenIteratingForwards = true;

            learner.Optimize(5, r);

            var score = runner.Score(learner.BestSpecies, runner.StateProvider.GetStateForNextGeneration());
            learner.BestSpecies.SaveToFile("C:\\ProjectLogs\\241.sg");

            var state = runner.StateProvider.GetStateForDemonstration();
            runner.Demonstrate(learner.BestSpecies, state);
        }
    }
}
