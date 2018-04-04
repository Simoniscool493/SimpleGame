using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SimplePacman.Payloads
{
    [Serializable()]
    class PacmanOutputInfo : DiscreteDataPayloadInfo
    {
        public PacmanOutputInfo()
        {
            valuePoints.Add(new ValuePoint(typeof(Direction)));
        }
    }
}
