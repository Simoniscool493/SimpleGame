using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics.GenAlg.Results
{
    class GenAlgSpecifiedGenerationNumTestResults : GenAlgTestResults
    {
        public int BestFinalScore => this.Select(r => r.BestScore).Max();
        public double AverageFinalScore => this.Select(r => r.BestScore).Average();
        public int WorstFinalScore => this.Select(r => r.BestScore).Min();

        public override string ToString()
        {
            return base.ToString() + $" Best: {BestFinalScore} Avg: {AverageFinalScore.ToString("0.0")} Worst: {WorstFinalScore}";
        }
    }
}
