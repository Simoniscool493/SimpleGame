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
        public double AverageTimeToScoreInSeconds => this.Select(r => r.TotalTimeTaken.TotalSeconds).Average();

        public int LeastGensToScore => (int)this.Select(r => r.GensToScore).Min();
        public double ShortestTimeToScoreInSeconds => this.Select(r => r.TotalTimeTaken.TotalSeconds).Min();

        public int MostGensToScore => (int)this.Select(r => r.GensToScore).Max();
        public double LongestTimeToScoreInSeconds => this.Select(r => r.TotalTimeTaken.TotalSeconds).Max();

        public override string ToString()
        {
            return base.ToString() + $" Avg:{AverageGensToScore.ToString("0.0")}g/{AverageTimeToScoreInSeconds.ToString("0.00")}s Best:{LeastGensToScore}g/{ShortestTimeToScoreInSeconds.ToString("0.00")}s Worst:{MostGensToScore}g/{LongestTimeToScoreInSeconds.ToString("0.00")}s";
        }
    }
}
