using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics
{
    class GenAlgTestResults : List<GenAlgTrainingSessionRecorder>
    {
        double AverageLearningPerSecond => this.Select(r => r.LearningPerSecond).Average();
        double AverageGenerationTimeInMillis => this.Select(r => r.AverageGenerationTime).Average();

        int BestFinalScore => this.Select(r => r.BestScore).Max();
        double AverageFinalScore => this.Select(r => r.BestScore).Average();
        int WorstFinalScore => this.Select(r => r.BestScore).Min();
    }
}
