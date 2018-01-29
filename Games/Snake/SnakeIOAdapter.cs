using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Games.Snake
{
    class SnakeIOAdapter : IDiscreteGameIOAdapter
    {
        public IDiscreteDataPayload GetOutput(IDiscreteGameState genericState)
        {
            var snakeState = (SnakeState)genericState;

            return new DiscreteDataPayload(snakeState.GetStatus());
        }

        public void SendInput(IDiscreteGameState genericState, IDiscreteDataPayload input)
        {
            ((SnakeState)genericState).SendInput(input.SingleItem);
        }
    }
}
