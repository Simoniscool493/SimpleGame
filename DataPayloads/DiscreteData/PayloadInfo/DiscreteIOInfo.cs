using System;

namespace SimpleGame.DataPayloads.DiscreteData
{
    [Serializable()]
    public class DiscreteIOInfo
    {
        public IDiscreteDataPayloadInfo InputInfo;
        public IDiscreteDataPayloadInfo OutputInfo;

        public DiscreteIOInfo(IDiscreteDataPayloadInfo inputInfo, IDiscreteDataPayloadInfo outputInfo)
        {
            InputInfo = inputInfo;
            OutputInfo = outputInfo;
        }
    }
}
