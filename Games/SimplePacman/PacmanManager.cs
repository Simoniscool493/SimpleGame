using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using System.Threading;
using log4net;

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

            Run(decider, state);
        }

        public int Run(IDiscreteDecider decider, IDiscreteGameState state)
        {
            while(true)
            {
                var status = IOADapter.GetOutput(state);

                if (status.Data[0]==PacmanConstants.GAME_OVER)
                {
                    state.Dispose();
                    return status.Data[1] + (status.Data[2]*255);
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state,direction);

                if(!(state is PacmanHeadlessInstance))
                {
                    Thread.Sleep(100);
                }
            }
        }

        public int RunWithLogging(ILog logger,IDiscreteDecider decider, IDiscreteGameState state)
        {
            logger.Debug("\n\nRunning a " + state.GetType() + "\n\n");
            while (true)
            {
                var status = IOADapter.GetOutput(state);
                logger.Debug("Game state is " + status.Data.Select(i => i.ToString()).Aggregate((i, j) => (i + " " + j)));

                if (status.Data[0] == PacmanConstants.GAME_OVER)
                {
                    logger.Debug("Game ended");
                    state.Dispose();
                    return status.Data[1] + (status.Data[2] * 255);
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state, direction);
                logger.Debug("Sent " + (Direction)direction.SingleItem);

                if (!(state is PacmanHeadlessInstance))
                {
                    Thread.Sleep(100);
                }
            }
        }


        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            return Run(decider, state);
        }
    }
}
