using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace SimpleGame.Metrics
{
    public class GeneticAlgorithmPerfromanceRecorder
    {
        public class GenerationData
        {
            public int BestScore;
            public int AverageScore;
            public long TimeTakenInMillis;

            public override string ToString()
            {
                return $"Best: {BestScore} Average: {AverageScore} Time in millis: {TimeTakenInMillis}";
            }
        }

        public double AverageIncreasePerGen => ((double)(Record.Last().AverageScore - Record.First().AverageScore)) / (double)Record.Count;
        public double AverageGenerationTime => Record.Select(gt => gt.TimeTakenInMillis).Average();

        public Stopwatch GenTimer;
        public List<GenerationData> Record;

        public GeneticAlgorithmPerfromanceRecorder()
        {
            Record = new List<GenerationData>();
            GenTimer = new Stopwatch();
        }

        public void LogGeneration(int avgScore,int bestScore,long timeTaken)
        {
            Record.Add(new GenerationData
            {
                BestScore = bestScore,
                AverageScore = avgScore,
                TimeTakenInMillis = timeTaken
            });
        }

        public override string ToString()
        {
            return $"AverageGenerationTime: {AverageGenerationTime} AverageIncreasePerGen: {AverageIncreasePerGen}";
        }
    }
}
