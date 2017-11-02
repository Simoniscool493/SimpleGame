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


        public void Mutate(Random r)
        {
            var outputEnumTypes = IOInfo.OutputInfo.PayloadType.GetEnumValues();
            var newExpectedOutput = (int)outputEnumTypes.GetValue(r.Next(0, outputEnumTypes.Length));

            ExpectedOutput = newExpectedOutput;

            /*var exceptionTask = r.Next(0, 3);
            if(exceptionTask == 0)
            {
                RemoveExceptions(1, r);
            }
            if(exceptionTask == 1)
            {
                AddExceptions(1, r);
            }*/
        }

        public void AddExceptions(int numExceptions,Random r)
        {
            for(int i=0; Exceptions.Count < MaxExceptions && i < numExceptions;i++)
            {
                var feature = IOInfo.InputInfo.GetSingleFeature(r);
                Exceptions.Add(feature);
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

        public static Heuristic CreateRandom(Random r, DiscreteIOInfo ioInfo,int numConditions,int numExceptions)
        {
            var expectedOutput = ioInfo.OutputInfo.GetRandomInstance(r).SingleItem;
            var h = new Heuristic(expectedOutput,ioInfo);

            h.AddConditions(numConditions, r);
            h.AddExceptions(numExceptions, r);
            return h;
        }

        public static Heuristic CreateHeuristicRandomlyFromThisInput(Random r, DiscreteIOInfo ioInfo,DiscreteDataPayload input,int numConditions)
        {
            var expectedOutput = ioInfo.OutputInfo.GetRandomInstance(r).SingleItem;
            var h = new Heuristic(expectedOutput, ioInfo);

            for (int i=0;i<numConditions;i++)
            {
                var position = r.Next(0, ioInfo.InputInfo.PayloadLength);
                var expectedInput = input.Data[position];
                h.Conditions.Add(new Tuple<int, int>(position, expectedInput));
            }

            return h;
        }

        public static Heuristic CreateExactHeuristicFromThisInput(Random r, DiscreteIOInfo ioInfo, DiscreteDataPayload input)
        {
            var expectedOutput = ioInfo.OutputInfo.GetRandomInstance(r).SingleItem;
            var h = new Heuristic(expectedOutput, ioInfo);

            for (int i = 0; i < ioInfo.InputInfo.PayloadLength; i++)
            {
                h.Conditions.Add(new Tuple<int,int>(i, input.Data[i]));
            }

            return h;
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
