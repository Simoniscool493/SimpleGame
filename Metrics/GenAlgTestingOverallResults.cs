using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Metrics
{
    class GenAlgTestingOverallResults : Dictionary<int[], GenAlgTestResults>
    {
        public void PrintToScreen()
        {
            foreach(var it in this)
            {
                string arrayAsString = "";

                foreach(int i in it.Key)
                {
                    arrayAsString = arrayAsString + i + " ";
                }

                Console.WriteLine(arrayAsString + " " + it.Value.ToString());
            }
        }
    }
}
