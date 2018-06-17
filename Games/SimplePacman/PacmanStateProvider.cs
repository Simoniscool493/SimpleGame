using Pacman;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SimplePacman
{
    class PacmanStateProvider : IDiscreteGameStateProvider
    {
        public IDiscreteGameState GetStateForDemonstration(int randomSeed)
        {
            return new PacmanDemoInstance(false,randomSeed);
        }

        public IDiscreteGameState GetStateForTraining(int randomSeed)
        {
            return new PacmanHeadlessInstance(randomSeed);
        }

        public IDiscreteGameState HookInToExistingState(int randomSeed)
        {
            return new PacmanDemoInstance(true,randomSeed);
        }

    }
}
