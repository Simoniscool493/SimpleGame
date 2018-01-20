using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame.DataPayloads.DiscreteData
{
    [Serializable()]
    public abstract class DiscreteDataPayloadInfo : IDiscreteDataPayloadInfo
    {
        public int PayloadLength => valuePoints.Count;
        public string[] dataPointNames;

        public bool IsSingle => (valuePoints.Count == 1);

        internal class ValuePoint
        {
            List<int> possibleValues = new List<int>();

            public ValuePoint(int[] values)
            {
                foreach (int i in values)
                {
                    possibleValues.Add(i);
                }
            }

            public ValuePoint(Type enumValues)
            {
                var values = enumValues.GetEnumValues();
                foreach (int i in values)
                {
                    possibleValues.Add(i);
                }
            }

            public int GetRandom(Random r)
            {
                return possibleValues.ElementAt(r.Next(0, possibleValues.Count));
            }

            public int GetDefault()
            {
                return possibleValues.First();
            }

        }

        internal List<ValuePoint> valuePoints = new List<ValuePoint>();

        public IDiscreteDataPayload GetRandomInstance(Random r)
        {
            int[] data = new int[PayloadLength];

            for(int i=0;i<PayloadLength;i++)
            {
                data[i] = valuePoints.ElementAt(i).GetRandom(r);
            }

            return new DiscreteDataPayload(data);
        }

        public IDiscreteDataPayload GetDefualtInstance()
        {
            int[] data = new int[PayloadLength];

            for (int i = 0; i < PayloadLength; i++)
            {
                data[i] = valuePoints.ElementAt(i).GetDefault();
            }

            return new DiscreteDataPayload(data);
        }

        public Tuple<int, int> GetSingleFeature(Random r)
        {
            throw new NotImplementedException();
        }
    }
}
