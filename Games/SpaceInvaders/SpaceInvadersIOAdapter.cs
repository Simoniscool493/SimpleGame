using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Games.SpaceInvaders.Instances;

namespace SimpleGame.Games.SpaceInvaders
{
    class SpaceInvadersIOAdapter : IDiscreteGameIOAdapter
    {
        public IDiscreteDataPayload GetOutput(IDiscreteGameState genericState)
        {
            var state = (ISpaceInvadersInstance)genericState;

            return new DiscreteDataPayload(state.GetStatus());

        }

        public void SendInput(IDiscreteGameState genericState, IDiscreteDataPayload input)
        {
            var state = (ISpaceInvadersInstance)genericState;

            state.SendInput(input.Data[0]);
        }
    }
}
