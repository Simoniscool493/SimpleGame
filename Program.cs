﻿
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders;
using SimpleGame.Deciders.DecisionMatrix;
using SimpleGame.Games;
using SimpleGame.Games.FoodEatingGame;
using SimpleGame.Games.SimplePacman;
using System;
using System.Collections.Generic;

namespace SimpleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var genAlg = new GeneticAlgorithmRunner
            (
                numGenerations: 100,
                numToKill: 5,
                numInGeneration: 15,
                numOfTimesToTestASpecies: 1,
                mutationRate: 0.1,
                deciderType: DeciderType.LazyMatrix
            );

            var runner = new PacmanManager();
            var stateProvider = (PacmanStateProvider)runner.StateProvider;

            //runner.Demonstrate(new SimpleGame.Deciders.Discrete.RandomDiscreteDecider(new Random(), runner.IOInfo), stateProvider.GetStateForDemonstration());

            var decider = genAlg.Train(runner, stateProvider, showProgress: false, demonstrateEveryXIterations: 0);

            runner.RunWithLogging(decider,stateProvider.GetStateForNextGeneration());

            runner.RunWithLogging(decider, stateProvider.GetStateForDemonstration());

            /*LazyDecisionMatrix m = new LazyDecisionMatrix(
                new Dictionary<DiscreteDataPayload, DiscreteDataPayload>(),
                new DiscreteIOInfo
                (
                    inputInfo: new DiscreteDataPayloadInfo(typeof(PacmanPointData), 8),
                    outputInfo: new DiscreteDataPayloadInfo(typeof(Direction), 1)
                ));

            var runner = new PacmanManager();
            var instance = new PacmanInstance();
            var score = runner.Score(m,instance);*/
        }
    }
}
