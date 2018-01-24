using SimpleGame.Deciders;
using SimpleGame.Games;
using SimpleGame.Metrics;

namespace SimpleGame.AI.GeneticAlgorithm
{
    public class EvaluationGeneticAlgorithmRunner : GeneticAlgorithmRunner
    {
        public GenAlgTrainingSessionRecorder Recorder;

        public EvaluationGeneticAlgorithmRunner(int numGenerations, int numToKill, int numInGeneration, int numOfTimesToTestASpecies, double mutationRate, DiscreteDeciderType deciderType, GenAlgTrainingSessionRecorder recorder)
            : base(numGenerations, numToKill, numInGeneration, numOfTimesToTestASpecies, mutationRate, deciderType)
        {
            Recorder = recorder;
        }

        protected override int RunGeneration(IDiscreteGameManager game, IDiscreteGameState state, Generation currentGeneration)
        {
            Recorder.GenTimer.Start();
            var gen = base.RunGeneration(game, state, currentGeneration);
            Recorder.GenTimer.Stop();

            Recorder.LogGeneration
            (
                currentGeneration.AverageScore,
                currentGeneration.BestSpecies.Score,
                Recorder.GenTimer.ElapsedMilliseconds
            );

            Recorder.GenTimer.Reset();

            if ((currentGeneration.BestSpecies.Score >= Recorder.ScoreToReach))
            {
                Recorder.GensToScore = _generationCounter;
                _earlyStopFlag = true;
            }

            return gen;
        }
    }
}
