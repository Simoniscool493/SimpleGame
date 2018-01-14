using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Games.SpaceInvaders
{
    class SpaceInvadersManager : IDiscreteGameManager
    {
        public DiscreteIOInfo IOInfo { get; }

        public IDiscreteGameIOAdapter IOADapter => new SpaceInvadersIOAdapter();

        public IDiscreteGameStateProvider StateProvider => new SpaceInvadersStateProvider();

        public SpaceInvadersManager()
        {
            IOInfo = new DiscreteIOInfo
            (
                inputInfo: new DiscreteDataPayloadInfo(null, 1, new string[] { "null",}),
                outputInfo: new DiscreteDataPayloadInfo(typeof(SpaceInvadersOutput), 1, new string[] { "output" })
            );
        }

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
