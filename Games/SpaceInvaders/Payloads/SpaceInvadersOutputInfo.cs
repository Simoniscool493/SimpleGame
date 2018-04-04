using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SpaceInvaders.Payloads
{
    [Serializable()]
    class SpaceInvadersOutputInfo : DiscreteDataPayloadInfo
    {
        public SpaceInvadersOutputInfo()
        {
            valuePoints.Add(new ValuePoint(typeof(SpaceInvadersOutput)));
        }
    }
}
