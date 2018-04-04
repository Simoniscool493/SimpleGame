using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SpaceInvaders.Payloads
{
    [Serializable()]
    class SpaceInvadersInputInfo : DiscreteDataPayloadInfo
    {
        public SpaceInvadersInputInfo()
        {
            valuePoints.Add(new ValuePoint(new []{ 0, 1 }));
            valuePoints.Add(new ValuePoint(new[] { 0, 1 }));
            valuePoints.Add(new ValuePoint(new[] { 0, 1 }));
            valuePoints.Add(new ValuePoint(new[] { 0, 6 }));

        }
    }
}
