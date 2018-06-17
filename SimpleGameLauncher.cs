
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
using System.IO;
using SimpleGame.Games.PokémonBattleEngine;
using System.Diagnostics;
using SimpleGame.Metrics;
using SimpleGame.Deciders.Discrete.HeuristicBuilder.Heuristic_Ensemble;

namespace SimpleGame
{
    class SimpleGameLauncher
    {
        public const string LogsPath = "C:\\ProjectLogs\\";
        public const string SavePath = "C:\\ProjectLogs\\Deciders\\";
        public const string BattlesPath = "C:\\ProjectLogs\\Deciders\\BattlesDecider.dc";

        static void Main(string[] args)
        {
            var logger = SimpleGameLoggerManager.SetupLogger();
            int choice = 0;
            int numIterations = 0;
            int generationSize = 0;
            int preferredMaxComplexity = 0;
            int preferredMinComplexity = 0;

            /*List<HeuristicBuildingDecider> deciders = new List<HeuristicBuildingDecider>();
            List<int> scores = new List<int>();
            List<int> scoresFinal = new List<int>();

            var manager = new PacmanManager(logger);

            for (int i=0;i<20;i++)
            {
                var decider = (HeuristicBuildingDecider)((DeciderSpecies)DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\Deciders\\" + i + ".dc")).BaseDecider;
                deciders.Add(decider);
                scores.Add((int)SimpleGameTester.SetRandomSuccessTesting(manager, decider, 1, i));
            }

            var ensemble = new HeuristicEnsembleDecider(deciders);
            var scoreFinal = (int)(SimpleGameTester.SetRandomSuccessTesting(manager, ensemble, 15, 0) / (15));

            var goodGeneral = (HeuristicBuildingDecider)((DeciderSpecies)DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\Deciders\\REALLYgoodGeneraldecider.dc")).BaseDecider;
            var goodGeneralScore = (int)(SimpleGameTester.UnsetRandomSuccessTesting(manager, goodGeneral, 100));

            var state = manager.StateProvider.GetStateForDemonstration(10);
            manager.Demonstrate(goodGeneral, state);*/




            /*for (int i=20;i<50;i++)
            {
                LearningSession
                (
                    logger: logger,
                    runner: new PacmanManager(logger),
                    numIterations: 15,
                    generationSize: 50, //10
                    preferredMaxComplexity: 10000,
                    preferredMinComplexity: 5000,
                    learningRate: 0.3, //0.3
                    numRandomSeeds: 1, //50
                    deciderConditionsToBuildFrom: 0,
                    saveAsFile: i.ToString(),
                    baseRandomSeed: i
                ); 
            }return;*/

            LearningSession
            (
                logger: logger,
                runner: new PacmanManager(logger),
                numIterations: 15,
                generationSize: 25, //10
                preferredMaxComplexity: 10000,
                preferredMinComplexity: 5000,
                learningRate: 0.3, //0.3
                numRandomSeeds: 50, //50
                deciderConditionsToBuildFrom: 5,
                saveAsFile: null,
                baseRandomSeed: 0
            );


            IDiscreteGameManager game;

            Console.WriteLine("Welcome to MindGame. Select a game to play.");
            Console.WriteLine("\t1. Snake.");
            Console.WriteLine("\t2. Pac-Man.");
            Console.WriteLine("\t3. FoodEatingGame. (Console animation - screen flashes when playing)");
            Console.WriteLine("\t4. Space Invaders.");
            Console.WriteLine("\t5. Battles.");

            Console.WriteLine("\n\t6. Preprepared Battles.");
            Console.WriteLine("\n\t7. Preprepared Snake.");
            Console.WriteLine("\t8. Preprepared Pac-Man.");

            while (true)
            {
                var isValid = Int32.TryParse(Console.ReadLine(), out choice);
                if(isValid && choice > 0 && choice < 9)
                {
                    break;
                }
            }

            switch(choice)
            {
                case 1:
                    game = new SnakeManager();
                    numIterations = 5; 
                    generationSize = 150;
                    preferredMaxComplexity = 2000;
                    preferredMinComplexity = 100;
                    LearningSession(logger, game, numIterations, generationSize, preferredMaxComplexity,preferredMinComplexity,0.2,1,0,null);

                    break;
                case 2:
                    game = new PacmanManager(logger);
                    numIterations = 80;
                    generationSize = 20;
                    preferredMaxComplexity = 10000;
                    preferredMinComplexity = 5000;
                    LearningSession(logger, game, numIterations, generationSize, preferredMaxComplexity, preferredMinComplexity, 0.3, 1, 0, null);

                    break;
                case 3:
                    game = new FoodEatingGameManager(100);
                    numIterations = 10;
                    generationSize = 100;
                    preferredMaxComplexity = 5000;
                    preferredMinComplexity = 100;
                    LearningSession(logger, game, numIterations, generationSize, preferredMaxComplexity, preferredMinComplexity, 0.2, 1, 0, null);

                    break;
                case 4:
                    game = new SpaceInvadersManager();
                    numIterations = 1;
                    generationSize = 10;
                    preferredMaxComplexity = 5000;
                    preferredMinComplexity = 10;
                    LearningSession(logger, game, numIterations, generationSize, preferredMaxComplexity, preferredMinComplexity, 0.2, 1, 0, null);
                    break;

                case 5:
                    game = new BattlesGameManager();
                    numIterations = 100;
                    generationSize = 10;
                    preferredMaxComplexity = 3000;
                    preferredMinComplexity = 1000;
                    LearningSession(logger, game, numIterations, generationSize, preferredMaxComplexity, preferredMinComplexity, 0.2, 1, 0, null);
                    break;

                case 6:
                    game = new BattlesGameManager();
                    Demo(game, BattlesPath);
                    break;
                case 7:
                    game = new SnakeManager();
                    Demo(game, Path.Combine(SavePath, "SnakeDecider.dc"));
                    break;
                case 8:
                    game = new PacmanManager(logger);
                    Demo(game, Path.Combine(SavePath, "PacmanDecider.dc"));

                    break;
            }

            Console.WriteLine("Finished.");
            Console.ReadLine();
        }

        public static void Demo(IDiscreteGameManager runner,string path)
        {
            var decider = DiscreteDeciderLoader.LoadFromFile(path);
            var state = runner.StateProvider.GetStateForDemonstration(0);
            runner.Demonstrate(decider, state);
        }

        public static void LearningSession(ILog logger,IDiscreteGameManager runner, int numIterations, int generationSize, int preferredMaxComplexity,int preferredMinComplexity,double learningRate,int numRandomSeeds,int deciderConditionsToBuildFrom,string saveAsFile,int baseRandomSeed = 0)
        {
            var r = new Random();

            var initSpecies = ((DeciderSpecies)DiscreteDeciderLoader.LoadFromFile("C:\\ProjectLogs\\Deciders\\REALLYgoodGeneraldecider.dc"));
            //var initSpecies = new DeciderSpecies(new HeuristicBuildingDecider(r, runner.IOInfo,deciderConditionsToBuildFrom));                           //Create decider
            var learner = new SinglePathMutationRunner(logger,r,runner,initSpecies,runner.StateProvider,true,false,numRandomSeeds, 5,2,generationSize,10,baseRandomSeed);         //Choose learning method

            //for(int i=0; i<numIterations;i++)
            while(true)
            {
                learner.Optimize(1, learningRate,preferredMaxComplexity,preferredMinComplexity);

                if(learner.BestSpeciesOverall.Score>23900)
                {
                    break;
                }
            }

            var scoreFinal = (int)(SimpleGameTester.SetRandomSuccessTesting(runner, learner.BestSpeciesOverall, 50, 0));

            //learner.GreedyOptimize(numIterations, r);

            //Console.WriteLine("Learning complete. Press enter to play game.");
            //Console.ReadLine();

            if (saveAsFile!=null)
            {
                learner.BestSpecies.SaveToFile(Path.Combine(SavePath, "better100" + ".dc"));
            }

            var state = runner.StateProvider.GetStateForDemonstration(baseRandomSeed);
            runner.Demonstrate(learner.BestSpeciesOverall, state);
        }

        public static void TimeTest(ILog logger)
        {
            var runner = new BattlesGameManager();
            for (int i = 0; i < 10; i++)
            {
                var watch = new Stopwatch();
                watch.Start();

                var r = new Random();

                var initSpecies = new DeciderSpecies(new HeuristicBuildingDecider(r, runner.IOInfo,0));
                var learner = new SinglePathMutationRunner(logger, r,runner, initSpecies, runner.StateProvider,false,false,1,5,2,10,10);

                for (int j = 0; j < 1; j++)
                {
                    learner.Optimize(1, 0.05, 1,0);
                }

                var time = watch.ElapsedMilliseconds / 1000d;
                Console.WriteLine(time);
            }

            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}
