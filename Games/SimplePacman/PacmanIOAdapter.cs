using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Games.SimplePacman
{
    class PacmanIOAdapter : IDiscreteGameIOAdapter
    {
        public DiscreteDataPayload GetOutput(IDiscreteGameState genericState)
        {
            throw new NotImplementedException();
        }

        public void SendInput(IDiscreteGameState genericState, DiscreteDataPayload input)
        {
            throw new NotImplementedException();
        }
    }
}
