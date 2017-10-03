using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SimplePacman
{
    class PacmanStateProvider : IDiscreteGameStateProvider
    {
        public IDiscreteGameState GetStateForDemonstration()
        {
            return new PacmanDemoInstance();
        }

        public IDiscreteGameState GetStateForNextGeneration()
        {
            return new PacmanHeadlessInstance();
        }
    }
}
