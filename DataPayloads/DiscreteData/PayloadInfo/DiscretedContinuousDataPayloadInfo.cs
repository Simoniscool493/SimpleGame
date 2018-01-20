using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.Iris
{
    public class DiscretedContinuousDataPayloadInfo : IDiscreteDataPayloadInfo
    {
        public int PayloadLength { get; }
        public int Granularity { get; }

        public bool HasType => false;
        public bool IsSingle => (PayloadLength == 1);

        public DiscretedContinuousDataPayloadInfo(int length, int granularity)
        {
            throw new NotImplementedException();

            PayloadLength = length;
            Granularity = granularity;

            int[] tempPossibleValues = new int[granularity];
            for(int i=0;i<granularity;i++)
            {
                tempPossibleValues[i] = i;
            }
        }

        public IDiscreteDataPayload GetDefualtInstance()
        {
            throw new NotImplementedException();
        }

        public IDiscreteDataPayload GetRandomInstance(Random r)
        {
            throw new NotImplementedException();
        }

        public Tuple<int, int> GetSingleFeature(Random r)
        {
            throw new NotImplementedException();
        }
    }
}
