using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using System.Diagnostics;

namespace SimpleGame.Deciders.HeuristicBuilder
{
    [Serializable()]
    public class Heuristic
    {
        public static int MaxConditions = 10;
        public static int MaxExceptions = 10;

        public List<Tuple<int, int>> Conditions;
        public List<Tuple<int, int>> Exceptions;

        public int ExpectedOutput;

        public int UseCount;
        public int ConsecutiveGensNotUsed;

        private DiscreteIOInfo IOInfo;

        public Heuristic(int exOutput,DiscreteIOInfo ioInfo)
        {
            Conditions = new List<Tuple<int, int>>();
            Exceptions = new List<Tuple<int, int>>();
            ExpectedOutput = exOutput;

            IOInfo = ioInfo;
            Exceptions = new List<Tuple<int, int>>();

            UseCount = 0;
            ConsecutiveGensNotUsed = 0;
        }

        public Heuristic GetCopy()
        {
            var newH = new Heuristic(ExpectedOutput, IOInfo);
            newH.Conditions = new List<Tuple<int, int>>(Conditions);
            newH.Exceptions = new List<Tuple<int, int>>(Exceptions);
            newH.UseCount = UseCount;
            newH.ConsecutiveGensNotUsed = ConsecutiveGensNotUsed;

            return newH;
        }

        public Heuristic GetMutated(Random r)
        {
            var newH = GetCopy();
            newH.Mutate(r);

            return newH;
        }

        public void Mutate(Random r)
        {
            var outputEnumTypes = IOInfo.OutputInfo.EnumValues;
            var newExpectedOutput = (int)outputEnumTypes.GetValue(r.Next(0, outputEnumTypes.Length));

            ExpectedOutput = newExpectedOutput;
            ConsecutiveGensNotUsed = 0;
            UseCount = 0;

            //TODO code to mutate conditons and exceptions
        }

        public void AddExceptions(int numExceptions,Random r)
        {
            for(int i=0;i<numExceptions;i++)
            {
                if(Conditions.Count >= IOInfo.InputInfo.PayloadLength || Exceptions.Count >= IOInfo.InputInfo.PayloadLength-1)
                {
                    return;
                }

                int position = -1;
                var conditionPositions = Conditions.Select(c => c.Item1);

                do
                {
                    position = r.Next(0, IOInfo.InputInfo.PayloadLength);
                }
                while (conditionPositions.Contains(position));

                Exceptions.Add(new Tuple<int, int>(position, IOInfo.OutputInfo.GetRandomInstance(r).SingleItem));
            }
        }

        public void RemoveExceptions(int numExceptions, Random r)
        {
            for (int i = 0; Exceptions.Any() && i < numExceptions; i++)
            {
                var randomException = r.Next(0, Exceptions.Count);
                Exceptions.RemoveAt(randomException);
            }
        }

        public void AddConditions(int numConditions, Random r)
        {
            for (int i = 0; Conditions.Count < MaxConditions && i < numConditions; i++)
            {
                var feature = IOInfo.InputInfo.GetSingleFeature(r);
                Conditions.Add(feature);
            }
        }

        public void RemoveConditions(int numConditions, Random r)
        {
            for (int i = 0; Conditions.Count > 1 && i < numConditions; i++)
            {
                var randomCondition = r.Next(0, Conditions.Count);
                Conditions.RemoveAt(randomCondition);
            }
        }
        
        public DiscreteDataPayload RecreatePayloadWithConditions()
        {
            int[] inputInfo = new int[IOInfo.InputInfo.PayloadLength];

            foreach(var h in Conditions)
            {
                inputInfo[h.Item1] = h.Item2;
            }

            return new DiscreteDataPayload(IOInfo.InputInfo.PayloadType,inputInfo);
        }


        public override string ToString()
        {
            var outputName = Enum.GetName(IOInfo.OutputInfo.PayloadType, ExpectedOutput);

            return $"Conditions: {Conditions.Count} Exceptions: {Exceptions.Count} Uses: {UseCount} => {outputName}";
        }
    }
}
