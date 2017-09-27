using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using System.Threading;

namespace SimpleGame.Games.SimplePacman
{
    class PacmanManager : IDiscreteGameManager
    {
        public DiscreteIOInfo IOInfo { get; }

        public IDiscreteGameStateProvider StateProvider { get; } = new PacmanStateProvider();

        public IDiscreteGameIOAdapter IOADapter { get; } = new PacmanIOAdapter();

        public PacmanManager()
        {
            IOInfo = new DiscreteIOInfo
            (
                inputInfo: new DiscreteDataPayloadInfo(typeof(PacmanPointData), 8),
                outputInfo: new DiscreteDataPayloadInfo(typeof(Direction), 1)
            );

        }
        public void Demonstrate(IDiscreteDecider decider, IDiscreteGameState state)
        {
        }

        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            var pacmanState = (PacmanInstance)state;
            state.Reset();

            while(true)
            {
                var status = IOADapter.GetOutput(state);

                if(status.SingleItem==PacmanConstants.GAME_OVER)
                {
                    return status.Data[1] + (status.Data[2]*255);
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state,direction);

                Thread.SpinWait(20);
            }
        }
    }
}
