using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.Deciders;
using SimpleGame.Games;
using SimpleGame.Metrics;

namespace SimpleGame.AI.GeneticAlgorithm
{
    public class EvaluationGeneticAlgorithmRunner : GeneticAlgorithmRunner
    {
        public GenAlgTrainingSessionRecorder Recorder;

        public EvaluationGeneticAlgorithmRunner(int numGenerations, int numToKill, int numInGeneration, int numOfTimesToTestASpecies, double mutationRate, DeciderType deciderType, GenAlgTrainingSessionRecorder recorder)
            : base(numGenerations, numToKill, numInGeneration, numOfTimesToTestASpecies, mutationRate, deciderType)
        {
            Recorder = recorder;
        }


        protected override int RunGeneration(IDiscreteGameManager game, IDiscreteGameState state, Generation currentGeneration)
        {
            Recorder.GenTimer.Start();
            var gen = base.RunGeneration(game, state, currentGeneration);
            Recorder.GenTimer.Stop();

            Recorder.LogGeneration(
                currentGeneration.GetAverageScore(),
                currentGeneration.GetBestSpecies().Score,
                Recorder.GenTimer.ElapsedMilliseconds);

            Recorder.GenTimer.Reset();

            return gen;
        }
    }
}
