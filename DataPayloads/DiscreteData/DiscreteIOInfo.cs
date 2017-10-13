using System;

namespace SimpleGame.DataPayloads.DiscreteData
{
    [Serializable()]
    public class DiscreteIOInfo
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
