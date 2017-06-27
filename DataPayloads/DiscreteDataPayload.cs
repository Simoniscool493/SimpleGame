using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.DataPayloads
{
    class DiscreteDataPayload
    {
        public int[] Data;

        public int SingleItem
        {
            get
            {
                if(Data.Length==1)
                {
                    return Data[0];
                }

                throw new Exception();
            }
        }

        public DiscreteDataPayload(int[] data)
        {
            Data = data;
        }

        public override bool Equals(object obj)
        {
            var payloadToCompare = (obj as DiscreteDataPayload);

            if(payloadToCompare != null)
            {
                return Data.SequenceEqual(payloadToCompare.Data);
            }

            return false;
        }
    }
}
