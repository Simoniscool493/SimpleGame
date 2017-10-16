using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics
{
    class GenAlgTestingOverallResults : Dictionary<int[], GenAlgTestResults>
    {
        public GenAlgTestingStartParamaters StartingParamaters;
        public int? ScoreToReach;

        public TimeSpan totalTimeTaken;

        static string[] paramaterNames = { "gens", "%ToKill", "genSize", "tests/Spec", "mut%", "dType" };

        public GenAlgTestingOverallResults(GenAlgTestingStartParamaters paramaters,int scoreToReach)
        {
            StartingParamaters = paramaters;

            if(scoreToReach>0)
            {
                ScoreToReach = scoreToReach;
            }
        }

        public void PrintAndLogGenerationTest(ILog logger)
        {
            Log(logger, GenerateStandardLogMessage());
        }

        public void PrintAndLogScoreTest(ILog logger)
        {
            string startMessage = GenerateStandardLogMessage() + " Score to reach was " + ScoreToReach + ".";

            Log(logger, startMessage);
        }

        private void Log(ILog logger,string startMessage)
        {
            logger.Debug(startMessage);
            Console.WriteLine(startMessage);

            foreach (var it in this)
            {
                string arrayAsString = "";

                for (int i = 0; i < it.Key.Length; i++)
                {
                    arrayAsString = arrayAsString + paramaterNames[i] + ":" + it.Key[i] + " ";
                }

                string toPrint = arrayAsString + " " + it.Value.ToString();

                logger.Debug(toPrint);
                Console.WriteLine(toPrint);
            }
        }

        private string GenerateStandardLogMessage()
        {
            return "Logging testing results. Each configuration tested " + StartingParamaters.TimesToTestEachConfiguration + " times. Total time taken: " + totalTimeTaken;
        }
    }
}
