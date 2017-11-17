using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.DataPayloads.DiscreteData
{
    public interface IDiscreteDataPayloadInfo
    {
        int PayloadLength { get; }

        Array PossibleValues { get; }

        bool HasType { get; }

        Type PayloadType { get; }

        IDiscreteDataPayload GetRandomInstance(Random r);

        IDiscreteDataPayload GetDefualtInstance();

        Tuple<int, int> GetSingleFeature(Random r);
    }
}
