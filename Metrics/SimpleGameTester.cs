using SimpleGame.AI;
using SimpleGame.AI.GeneticAlgorithm;
using SimpleGame.Deciders;
using SimpleGame.Games;
using SimpleGame.Games.SimplePacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics
{
    class SimpleGameTester
    {
        public static GenAlgTestingOverallResults TestGeneticAlgorithm(GenAlgTestingStartParamaters parameterList, IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider)
        {
            var output = new GenAlgTestingOverallResults(parameterList);
            var allPermutations = parameterList.GetAllPermutations();

            foreach(var perm in allPermutations)
            {
                output[perm] = TestSingleSetOfParamaters(parameterList.TimesToTestEachConfiguration, parameterList.IncrementToRecord, perm, runner, stateProvider);
            }

            return output;
        }

        private static GenAlgTestResults TestSingleSetOfParamaters(int timesToTest,int incrementToRecord,int[] paramaters, IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider)
        {
            var tests = new GenAlgTestResults();

            var numGenerationsParamater = paramaters[0];
            var percentToKillParamater = paramaters[1];
            var generationSizeParamater = paramaters[2];
            var iterationsOfTestingPerSpeciesParamater = paramaters[3];
            var mutationPercentParamater = paramaters[4];
            var deciderTypeParamater = (DeciderType)paramaters[5];

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
                var recorder = RunSingleGeneticAlgorithmTrainingSession(runner, stateProvider, genAlg, incrementToRecord);
                tests.Add(recorder);
            }

            return tests;
        }

        private static GenAlgTrainingSessionRecorder RunSingleGeneticAlgorithmTrainingSession(IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider, EvaluationGeneticAlgorithmRunner genAlg,int incrementToTestWith)
        {
            var recorder = new GenAlgTrainingSessionRecorder(incrementToTestWith);
            genAlg.Recorder = recorder;

            recorder.TotalTimer.Start();
            genAlg.Train(runner, stateProvider, false,false, 0);
            recorder.TotalTimer.Stop();

            recorder.TotalTimeTaken = recorder.TotalTimer.Elapsed;

            return recorder;
        }
    }
}
