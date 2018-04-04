using Pacman;
using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SimplePacman.Payloads
{
    [Serializable()]
    class PacmanInputInfo : DiscreteDataPayloadInfo
    {
        public PacmanInputInfo()
        {
            for(int i=0;i< 40;i++)
            {
                valuePoints.Add(new ValuePoint(typeof(PacmanPointData)));
            }

            /*for (int i = 0; i < 4; i++)
            {
                valuePoints.Add(new ValuePoint(new[] { 0, 1 }));
            }*/
        }
    }
}
