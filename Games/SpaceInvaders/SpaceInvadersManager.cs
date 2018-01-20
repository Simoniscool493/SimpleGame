using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games.SpaceInvaders.Payloads;
using SimpleGame.Games.SpaceInvaders.Instances;
using System.Threading;

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
                inputInfo: new SpaceInvadersInputInfo(),
                outputInfo: new SpaceInvadersOutputInfo()
            );
        }

        public void Demonstrate(IDiscreteDecider decider, IDiscreteGameState state)
        {
            int steps = 0;

            while (true)
            {
                var status = IOADapter.GetOutput(state);

                if (status.Data[0] == SpaceInvadersConstants.GAME_OVER)
                {
                    state.Dispose();
                    return;
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state, direction);

                if (!(state is SpaceInvadersHeadlessInstance))
                {
                    Thread.Sleep(75);
                }

                if (steps++ > 20000)
                {
                    Console.WriteLine();
                }
            }
        }

        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            state.Reset();
            int steps = 0;

            while (true)
            {
                var status = IOADapter.GetOutput(state);

                if (status.Data[0] == SpaceInvadersConstants.GAME_OVER)
                {
                    state.Dispose();
                    return status.Data[1] + (status.Data[2] * 255);
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state, direction);

                if (!(state is SpaceInvadersHeadlessInstance))
                {
                    Thread.Sleep(75);
                }

                if(steps++>20000)
                {
                    Console.WriteLine();
                }
            }
        }
    }
}
