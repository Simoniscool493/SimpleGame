using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame
{
    class CSVWriter
    {
        public static void WriteSinglePathMutationRunnerCSV(string fileIn,string fileOut)
        {
            string[] lines = File.ReadAllLines(fileIn);
            StreamWriter writetext = new StreamWriter(fileOut);
            string config = "config Not found";

            writetext.WriteLine("seqNumber,type,score,genes,complexity");

            foreach (string line in lines)
            {
                if (line.Contains("RawData"))
                {
                    var tokens = line.Split(',');
                    int seqNumber = int.Parse(tokens[1]);
                    int type = int.Parse(tokens[2]);
                    int score = int.Parse(tokens[3]);
                    int genes = int.Parse(tokens[4]);
                    int complexity = int.Parse(tokens[5]);

                    writetext.WriteLine(seqNumber + "," + type + "," + score + "," + genes + "," + complexity);
                }
                else if (line.Contains("MutationRunnerConfig"))
                {
                    config = line;
                }
            }

            writetext.WriteLine(config);
            writetext.Close();
        }
    }
}
