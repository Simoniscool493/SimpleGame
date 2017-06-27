using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.DataPayloads
{
    class DiscreteIOInfo
    {
        public DiscreteDataPayloadInfo InputInfo;
        public DiscreteDataPayloadInfo OutputInfo;

        public DiscreteIOInfo(DiscreteDataPayloadInfo inputInfo,DiscreteDataPayloadInfo outputInfo)
        {
            InputInfo = inputInfo;
            OutputInfo = outputInfo;
        }
    }
}
