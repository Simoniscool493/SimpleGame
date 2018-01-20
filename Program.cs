
using SimpleGame.Deciders;
using SimpleGame.Games.SimplePacman;
using System;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Deciders.HeuristicBuilder;
using SimpleGame.AI;
using SimpleGame.Games.SpaceInvaders;
using System.Collections.Generic;
using System.Threading;

namespace SimpleGame
{
    class Program
    {
        public const string LogsPath = "C:\\ProjectLogs\\";

        static void Main(string[] args)
        {
            var runner = new SpaceInvadersManager();
            var state = runner.StateProvider.GetStateForDemonstration();
            var decider = new RandomDiscreteDecider(new Random(), runner.IOInfo);

            Thread.Sleep(10000);
            runner.Demonstrate(decider, state);

            List<int> scores = new List<int>();

            for(int i=0;i<10;i++)
            {
                var score = runner.Score(decider, state);

                scores.Add(score);
            }

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

            learner.GenerationSize = 4;        //25
            learner.MaxHeuristicsToTake = 10;   //10
            learner.TimesToTestPerSpecies = 1;  //100
            learner.MinimizeComplexity = true;
            learner.IncludePreviousBestWhenIteratingForwards = true;

            learner.Optimize(1, r);

            var score = runner.Score(learner.BestSpecies, runner.StateProvider.GetStateForNextGeneration());
            //learner.BestSpecies.SaveToFile("C:\\ProjectLogs\\241.sg");

            var state = runner.StateProvider.GetStateForDemonstration();
            runner.Demonstrate(learner.BestSpecies, state);
        }
    }
}
