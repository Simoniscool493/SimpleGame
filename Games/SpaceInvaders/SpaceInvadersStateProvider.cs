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
        public int RandomSeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDiscreteGameState GetStateForDemonstration()
        {
            return new SpaceInvadersDemoInstance(false);
        }

        public IDiscreteGameState GetStateForNextGeneration()
        {
            return new SpaceInvadersHeadlessInstance();
        }
    }
}
