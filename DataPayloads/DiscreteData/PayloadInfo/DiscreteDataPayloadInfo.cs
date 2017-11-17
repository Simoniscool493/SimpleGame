using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame.DataPayloads.DiscreteData
{
    [Serializable()]
    public class DiscreteDataPayloadInfo : IDiscreteDataPayloadInfo
    {
        public int PayloadLength { get; }

        public bool HasType => true;
        public Type PayloadType { get; }
        public Array PossibleValues { get; private set; }

        public string[] PositionNames;

        public DiscreteDataPayloadInfo(Type pType,int pLength,string[] positionNames)
        {
            PayloadLength = pLength;
            PayloadType = pType;

            PositionNames = positionNames;

            PossibleValues = PayloadType.GetEnumValues();
        }

        public IDiscreteDataPayload GetRandomInstance(Random r)
        {
            if(PossibleValues == null)
            {
                PossibleValues = PayloadType.GetEnumValues();
            }

            if(PayloadType.IsEnum)
            {
                var output = new List<int>();

                for(int i=0;i< PayloadLength; i++)
                {
                    output.Add((int)PossibleValues.GetValue(r.Next(0, PossibleValues.Length)));
                }

                return new DiscreteDataPayload(PayloadType,output.ToArray());
            }

            throw new Exception();
        }

        public Tuple<int,int> GetSingleFeature(Random r)
        {
            var position = r.Next(0, PayloadLength);
            var value = (int)PossibleValues.GetValue(r.Next(0, PossibleValues.Length));

            return new Tuple<int, int>(position, value);
        }

        public IDiscreteDataPayload GetDefualtInstance()
        {
            if (PayloadType.IsEnum)
            {
                var value = (int)PossibleValues.GetValue(0);

                return new DiscreteDataPayload(PayloadType, value);
            }

            throw new Exception();
        }
    }
}
