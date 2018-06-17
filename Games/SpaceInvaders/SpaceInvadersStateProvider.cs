using SimpleGame.Games.SpaceInvaders.Instances;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SpaceInvaders
{
    class SpaceInvadersStateProvider : IDiscreteGameStateProvider
    {
        public IDiscreteGameState GetStateForDemonstration(int randomSeed)
        {
            return new SpaceInvadersDemoInstance(false,randomSeed);
        }

        public IDiscreteGameState GetStateForTraining(int randomSeed)
        {
            return new SpaceInvadersHeadlessInstance(randomSeed);
        }
    }
}
