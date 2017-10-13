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

        public override string ToString()
        {
            return $"LPS: {AverageLearningPerSecond.ToString("0.0")} Gentime: {AverageGenerationTimeInMillis.ToString("0.0")}";
        }
    }
}
