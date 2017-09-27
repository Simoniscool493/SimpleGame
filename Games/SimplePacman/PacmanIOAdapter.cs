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
            var state = (PacmanInstance)genericState;

            return new DiscreteDataPayload(typeof(PacmanPointData), state.GetStatus());

        }

        public void SendInput(IDiscreteGameState genericState, DiscreteDataPayload input)
        {
            var state = (PacmanInstance)genericState;

            state.SendInput((Direction)input.Data[0]);
        }
    }
}
