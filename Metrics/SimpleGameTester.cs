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
                numGenerations: 100,
                numToKill: 13,
                numInGeneration: 20,
                numOfTimesToTestASpecies: 1,
                mutationRate: 0.2,
                deciderType: DeciderType.LazyMatrix,
                recorder: null
            );

            var result1 = TestThisConfiguration(5, runner, stateProvider, genAlg);

            var result2 = TestThisConfiguration(10, runner, stateProvider, genAlg);

            var result3 = TestThisConfiguration(20, runner, stateProvider, genAlg);





        }

        public GeneticAlgorithmPerformanceResult TestThisConfiguration(int timesToTest,IDiscreteGameManager runner, IDiscreteGameStateProvider stateProvider,EvaluationGeneticAlgorithmRunner genAlg)
        {
            var tests = new List<GeneticAlgorithmPerfromanceRecorder>();

            for (int i = 0; i < timesToTest; i++)
            {
                var recorder1 = new GeneticAlgorithmPerfromanceRecorder();
                genAlg.Recorder = recorder1;
                genAlg.Train(runner, stateProvider, false, 0);
                tests.Add(recorder1);
            }

            return new GeneticAlgorithmPerformanceResult(tests.Select(t => t.AverageIncreasePerGen).Average(),tests.Select(t => t.AverageGenerationTime).Average());
        }

        public class GeneticAlgorithmPerformanceResult
        {
            public double AverageIncreasePerGen;
            public double AverageGenerationTime;

            public GeneticAlgorithmPerformanceResult(double averageIncreasePerGen,double averageGenerationTime)
            {
                AverageGenerationTime = averageGenerationTime;
                AverageIncreasePerGen = averageIncreasePerGen;
            }

            public override string ToString()
            {
                return $"AverageGenerationTime: {AverageGenerationTime.ToString("0.00")} AverageIncreasePerGen: {AverageIncreasePerGen.ToString("0.00")}";
            }
        }
    }
}
