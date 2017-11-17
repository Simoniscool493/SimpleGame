using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.DataPayloads.DiscreteData
{
    public class DiscretedContinuousDataPayload : IDiscreteDataPayload
    {
        public int[] Data { get; }

        public int SingleItem
        {
            get
            {
                if (Data.Length == 1)
                {
                    return Data[0];
                }

                throw new Exception();
            }
        }

        public DiscretedContinuousDataPayload(int[] data)
        {
            Data = data;
        }
    }
}
