using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics.GenAlg.Results
{
    class GenAlgToScoreTestResults : GenAlgTestResults
    {
        public double AverageGensToScore => (double)this.Select(r => r.GensToScore).Average();
        public int ShortestGensToScore => (int)this.Select(r => r.GensToScore).Min();
        public double AverageTimeToScoreInSeconds => this.Select(r => r.TotalTimeTaken.TotalSeconds).Average();
        public double ShortestTimeToScoreInSeconds => this.Select(r => r.TotalTimeTaken.TotalSeconds).Min();

        public override string ToString()
        {
            return base.ToString() + $" AvGens:{AverageGensToScore.ToString("0.00")} AvTime:{AverageTimeToScoreInSeconds.ToString("0.00")}s LeastGens:{ShortestGensToScore} LeastTime: {ShortestTimeToScoreInSeconds.ToString("0.00")}s";
        }
    }
}
