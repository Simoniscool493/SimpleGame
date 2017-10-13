using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
namespace SimpleGame.Metrics
{
    public class GenAlgTrainingSessionRecorder
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

        public int? ScoreToReach;
        public int? GensToScore;

        public double AverageIncreasePerGen => ((double)(Record.Last().AverageScore - Record.First().AverageScore)) / (double)Record.Count;
        public double AverageGenerationTime => Record.Select(gt => gt.TimeTakenInMillis).Average();
        public double LearningPerSecond
        {
            get
            {
                if(TotalTimeTaken.TotalSeconds==0)
                {
                    return 0;
                }

                return ((double)(Record.Last().BestScore) - (double)(Record.First().BestScore)) / (double)(TotalTimeTaken.TotalSeconds);
            }
        }
        public int BestScore => Record.Last().BestScore;

        public TimeSpan TotalTimeTaken;

        public Stopwatch TotalTimer;
        public Stopwatch GenTimer;
        public List<GenerationData> Record;
        public List<GenAlgSnapshot> Results;

        private int _incrementToTestWith;
        private int _generationCounter = 0;

        public GenAlgTrainingSessionRecorder(int incrementToTestWith,int scoreToReach)
        {
            _incrementToTestWith = incrementToTestWith;

            Record = new List<GenerationData>();
            Results = new List<GenAlgSnapshot>();
            GenTimer = new Stopwatch();
            TotalTimer = new Stopwatch();

            if(scoreToReach>0)
            {
                ScoreToReach = scoreToReach;
            }
        }

        public void LogGeneration(int avgScore,int bestScore,long timeTaken)
        {
            Record.Add(new GenerationData
            {
                BestScore = bestScore,
                AverageScore = avgScore,
                TimeTakenInMillis = timeTaken
            });

            if(_incrementToTestWith > 0 && _generationCounter % _incrementToTestWith==0)
            {
                var result = new GenAlgSnapshot(_generationCounter, AverageIncreasePerGen,AverageGenerationTime,LearningPerSecond,bestScore);
                Results.Add(result);
            }

            _generationCounter++;
        }

        public override string ToString()
        {
            return $"Best: {BestScore} Inc: {AverageIncreasePerGen} LPS: {LearningPerSecond.ToString("0")} AverageGenerationTime: {AverageGenerationTime} TotalTime: {TotalTimeTaken}";
        }
    }
}
