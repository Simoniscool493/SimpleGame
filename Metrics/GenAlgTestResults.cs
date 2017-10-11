using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics
{
    class GenAlgTestResults : List<GenAlgTrainingSessionRecorder>
    {
        public double AverageLearningPerSecond => this.Select(r => r.LearningPerSecond).Average();
        public double AverageGenerationTimeInMillis => this.Select(r => r.AverageGenerationTime).Average();

        public int BestFinalScore => this.Select(r => r.BestScore).Max();
        public double AverageFinalScore => this.Select(r => r.BestScore).Average();
        public int WorstFinalScore => this.Select(r => r.BestScore).Min();

        public override string ToString()
        {
            return $"LPS: {AverageLearningPerSecond.ToString("0.0")} Best: {BestFinalScore} Avg: {AverageFinalScore} Worst: {WorstFinalScore} Gentime: {AverageGenerationTimeInMillis.ToString("0.0")}";
        }
    }
}
