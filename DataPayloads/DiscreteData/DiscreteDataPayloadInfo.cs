using System;
using System.Collections.Generic;

namespace SimpleGame.DataPayloads.DiscreteData
{
    [Serializable()]
    public class DiscreteDataPayloadInfo
    {
        public int PayloadLength;
        public Type PayloadType;

        public DiscreteDataPayloadInfo(Type pType,int pLength)
        {
            PayloadLength = pLength;
            PayloadType = pType;
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
    }
}
