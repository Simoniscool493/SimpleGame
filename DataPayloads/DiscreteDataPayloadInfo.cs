using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.DataPayloads
{
    class DiscreteDataPayloadInfo
    {
        private int payloadLength;
        private Type payloadType;

        public DiscreteDataPayloadInfo(Type pType,int pLength)
        {
            payloadLength = pLength;
            payloadType = pType;
        }

        public DiscreteDataPayload GetRandomInstance(Random r)
        {
            if(payloadType.IsEnum)
            {
                var values = payloadType.GetEnumValues();
                var output = new List<int>();

                for(int i=0;i<payloadLength;i++)
                {
                    output.Add(r.Next(0, values.Length));
                }

                return new DiscreteDataPayload(output.ToArray());
            }

            throw new Exception();
        }
    }
}
