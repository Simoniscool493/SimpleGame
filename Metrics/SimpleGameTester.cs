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
        public void GeneticAlgorithmTests(IDiscreteGameManager runner,IDiscreteGameStateProvider stateProvider)
        {
            var genAlg = new EvaluationGeneticAlgorithmRunner
            (
                numGenerations: 5,
                numToKill: 13,
                numInGeneration: 20,
                numOfTimesToTestASpecies: 1,
                mutationRate: 0.2,
                deciderType: DeciderType.LazyMatrix,
                recorder: null
            );

            var result2 = TestGeneticAlgorithm
            (
                runner, stateProvider, genAlg, 
                timesToTest: 100, 
                incrementToTestWith: 100
            );

        }

        public GenAlgTestResults TestGeneticAlgorithm(IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider,EvaluationGeneticAlgorithmRunner genAlg,
            int timesToTest, int incrementToTestWith)
        {
            var tests = new GenAlgTestResults();

            for (int i = 0; i < timesToTest; i++)
            {
                var recorder = RunSingleGeneticAlgorithmTrainingSession(runner, stateProvider, genAlg, incrementToTestWith);
                tests.Add(recorder);
            }

            return tests;
        }

        private GenAlgTrainingSessionRecorder RunSingleGeneticAlgorithmTrainingSession(IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider, EvaluationGeneticAlgorithmRunner genAlg,int incrementToTestWith)
        {
            var recorder = new GenAlgTrainingSessionRecorder(incrementToTestWith);
            genAlg.Recorder = recorder;

            recorder.TotalTimer.Start();
            genAlg.Train(runner, stateProvider, false, 0);
            recorder.TotalTimer.Stop();

            recorder.TotalTimeTaken = recorder.TotalTimer.Elapsed;

            return recorder;
        }
    }
}
