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
            var pacmanState = state as PacmanDemoInstance;
            if(pacmanState==null)
            {
                throw new Exception("This is not a demonstration state");
            }
            state.Reset();

            while (true)
            {
                var status = IOADapter.GetOutput(state);

                if (status.Data[0] == PacmanConstants.GAME_OVER)
                {
                    return;
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state, direction);

                Thread.Sleep(250);
            }
        }

        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            var pacmanState = (IPacmanInstance)state;
            state.Reset();
            
            while(true)
            {
                var status = IOADapter.GetOutput(state);

                if(status.Data[0]==PacmanConstants.GAME_OVER)
                {
                    return status.Data[1] + (status.Data[2]*255);
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state,direction);

                if(pacmanState is PacmanHeadlessInstance)
                {
                    ((PacmanHeadlessInstance)state).Tick();
                }
                else
                {
                    Thread.Sleep(250);
                }
            }
        }
    }
}
