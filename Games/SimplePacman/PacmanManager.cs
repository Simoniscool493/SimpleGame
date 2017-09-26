using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Games.SimplePacman
{
    class PacmanManager : IDiscreteGameManager
    {
        public DiscreteIOInfo IOInfo => throw new NotImplementedException();

        public IDiscreteGameIOAdapter IOADapter => throw new NotImplementedException();

        public void Demonstrate(IDiscreteDecider decider, IDiscreteGameState state)
        {
            throw new NotImplementedException();
        }

        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            throw new NotImplementedException();
        }
    }
}
