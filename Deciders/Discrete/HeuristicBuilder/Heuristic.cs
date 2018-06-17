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

        public int[] Conditions;

        public int ExpectedOutput;
        public int UseCount;
        public int ConsecutiveGensNotUsed;

        private DiscreteIOInfo IOInfo;

        public Heuristic(int exOutput, DiscreteIOInfo ioInfo)
        {
            Conditions = new int[ioInfo.InputInfo.PayloadLength];
            for (int i = 0; i < Conditions.Length; i++)
            {
                Conditions[i] = -1;
            }

            ExpectedOutput = exOutput;

            IOInfo = ioInfo;

            ConsecutiveGensNotUsed = 0;
        }

        public Heuristic GetCopy(bool copyTemporaryData)
        {
            var newH = new Heuristic(ExpectedOutput, IOInfo) { Conditions = (int[])Conditions.Clone() };

            if(copyTemporaryData)
            {
                newH.ConsecutiveGensNotUsed = ConsecutiveGensNotUsed;
                newH.UseCount = UseCount;
            }

            return newH;
        }

        public Heuristic GetMutated(Random r)
        {
            var newH = GetCopy(true);
            newH.Mutate(r);

            return newH;
        }

        public void Mutate(Random r)
        {
            var newExpectedOutput = IOInfo.OutputInfo.GetRandomInstance(r).SingleItem;

            ExpectedOutput = newExpectedOutput;
            ConsecutiveGensNotUsed = 0;
            //TODO code to mutate conditons and exceptions
        }

        public void AddConditions(int numConditions, Random r)
        {
            for (int i = 0; Conditions.Length < MaxConditions && i < numConditions; i++)
            {
                var feature = IOInfo.InputInfo.GetSingleFeature(r);
                Conditions[feature.Item1] = feature.Item2;
            }
        }

        public void RemoveConditions(int numConditions, Random r)
        {
            for (int i = 0; Conditions.Length > 1 && i < numConditions; i++)
            {
                var randomCondition = r.Next(0, Conditions.Length);
                Conditions[randomCondition] = -1;
            }
        }

        public DiscreteDataPayload RecreatePayloadWithConditions()
        {
            int[] inputInfo = new int[IOInfo.InputInfo.PayloadLength];

            for (int i = 0; i < Conditions.Length; i++)
            {
                if (Conditions[i] == -1)
                {
                    throw new Exception();
                }

                inputInfo[i] = Conditions[i];
            }

            return new DiscreteDataPayload(inputInfo);
        }

        public override string ToString()
        {
            return $"Conditions: {Conditions.Length} => <TODO>";
        }
    }
}
