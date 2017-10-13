using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics
{
    public class GenAlgSnapshot
    {
        public int GenerationNumber;
        public double AverageIncreasePerGen;
        public double AverageGenerationTime;
        public double AverageLearningPerSecond;
        public double AverageBest;

        public GenAlgSnapshot(int generationNumber, double averageIncreasePerGen, double averageGenerationTime, double averageLearningPerSecond, double averageBest)
        {
            GenerationNumber = generationNumber;
            AverageGenerationTime = averageGenerationTime;
            AverageIncreasePerGen = averageIncreasePerGen;
            AverageLearningPerSecond = averageLearningPerSecond;
            AverageBest = averageBest;
        }

        public override string ToString()
        {
            return $"Generation: {GenerationNumber} " +
                   $"GenerationTime: {AverageGenerationTime.ToString("0.0")}ms " +
                   $"IncreasePerGen: {AverageIncreasePerGen.ToString("0.00")} " +
                   $"AverageBest: {AverageBest.ToString("0.00")} " +
                   $"LearningPerSecond: {AverageLearningPerSecond.ToString("0.00")}";
        }
    }
}
