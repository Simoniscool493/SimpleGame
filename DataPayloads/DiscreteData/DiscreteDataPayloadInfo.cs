using System;
using System.Collections.Generic;
using System.Linq;

namespace SimpleGame.DataPayloads.DiscreteData
{
    [Serializable()]
    public class DiscreteDataPayloadInfo
    {
        public int PayloadLength;

        public Type PayloadType;
        public Array EnumValues;

        public string[] PositionNames;

        public DiscreteDataPayloadInfo(Type pType,int pLength,string[] positionNames)
        {
            PayloadLength = pLength;
            PayloadType = pType;

            PositionNames = positionNames;

            EnumValues = PayloadType.GetEnumValues();
        }

        public DiscreteDataPayload GetRandomInstance(Random r)
        {
            if(PayloadType.IsEnum)
            {
                var output = new List<int>();

                for(int i=0;i< PayloadLength; i++)
                {
                    output.Add((int)EnumValues.GetValue(r.Next(0, EnumValues.Length)));
                }

                return new DiscreteDataPayload(PayloadType,output.ToArray());
            }

            throw new Exception();
        }

        public Tuple<int,int> GetSingleFeature(Random r)
        {
            var position = r.Next(0, PayloadLength);
            var value = (int)EnumValues.GetValue(r.Next(0, EnumValues.Length));

            return new Tuple<int, int>(position, value);
        }

        public DiscreteDataPayload GetDefualtInstance()
        {
            if (PayloadType.IsEnum)
            {
                var value = (int)EnumValues.GetValue(0);

                return new DiscreteDataPayload(PayloadType, value);
            }

            throw new Exception();
        }
    }
}
