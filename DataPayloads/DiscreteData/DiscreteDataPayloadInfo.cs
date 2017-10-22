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

        public string[] PositionNames;

        public DiscreteDataPayloadInfo(Type pType,int pLength,string[] positionNames)
        {
            PayloadLength = pLength;
            PayloadType = pType;

            PositionNames = positionNames;
        }

        public DiscreteDataPayload GetRandomInstance(Random r)
        {
            if(PayloadType.IsEnum)
            {
                var values = PayloadType.GetEnumValues();
                var output = new List<int>();

                for(int i=0;i< PayloadLength; i++)
                {
                    output.Add((int)values.GetValue(r.Next(0, values.Length)));
                }

                return new DiscreteDataPayload(PayloadType,output.ToArray());
            }

            throw new Exception();
        }

        public DiscreteDataPayload GetDefualtInstance()
        {
            if (PayloadType.IsEnum)
            {
                var value = (int)PayloadType.GetEnumValues().GetValue(0);

                return new DiscreteDataPayload(PayloadType, value);
            }

            throw new Exception();
        }
    }
}
