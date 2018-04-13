
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
using SimpleGame.Games;

namespace SimpleGame
{
    class Program
    {
        public const string LogsPath = "C:\\ProjectLogs\\";

        static void Main(string[] args)
        {
            var logger = SimpleGameLoggerManager.SetupLogger();
            int choice = 0;
            int numIterations = 0;
            int generationSize = 0;
            IDiscreteGameManager game;

            Console.WriteLine("Welcome to MindGame. Select a game to play.");
            Console.WriteLine("\t1. Snake.");
            Console.WriteLine("\t2. Pac-Man.");
            Console.WriteLine("\t3. FoodEatingGame. (Console animation - screen flashes when playing)");
            Console.WriteLine("\t4. Space Invaders.");

            while(true)
            {
                var isValid = Int32.TryParse(Console.ReadLine(), out choice);
                if(isValid && choice > 0 && choice < 5)
                {
                    break;
                }
            }

            switch(choice)
            {
                case 1:
                    game = new SnakeManager();
                    numIterations = 15;
                    generationSize = 50;
                    break;
                case 2:
                    game = new PacmanManager(logger);
                    numIterations = 8;
                    generationSize = 45;
                    break;
                case 3:
                    game = new FoodEatingGameManager(100);
                    numIterations = 10;
                    generationSize = 100;
                    break;
                default:
                    game = new SpaceInvadersManager();
                    numIterations = 1;
                    generationSize = 10;
                    break;

            }

            LearningSession(logger,game,numIterations,generationSize);

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }

        public static void LearningSession(ILog logger,IDiscreteGameManager runner, int numIterations, int generationSize)
        {
            var r = new Random();

            var learner = new SinglePathMutationRunner(logger,runner, runner.StateProvider, true, true, true);      //Choose learning method
            learner.BestSpecies = new DeciderSpecies(new HeuristicBuildingDecider(r, runner.IOInfo));               //Create decider

            learner.GenerationSize = generationSize;
            learner.MaxHeuristicsToTake = 10;
            learner.TimesToTestPerSpecies = 1;
            learner.MinimizeComplexity = true;
            learner.IncludePreviousBestWhenIteratingForwards = true;

            learner.Optimize(numIterations, 0.1, r);

            Console.WriteLine("Learning complete. Press enter to play game.");
            Console.ReadLine();

            var state = runner.StateProvider.GetStateForDemonstration();
            runner.Demonstrate(learner.BestSpecies, state);
        }
    }
}
