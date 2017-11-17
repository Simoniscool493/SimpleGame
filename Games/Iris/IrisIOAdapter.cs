using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Games.Iris
{
    class IrisIOAdapter : IDiscreteGameIOAdapter
    {
        public void SendInput(IDiscreteGameState genericState, IDiscreteDataPayload input)
        {
            throw new NotImplementedException();
        }

        public IDiscreteDataPayload GetOutput(IDiscreteGameState genericState)
        {
            throw new NotImplementedException();
        }
    }
}
