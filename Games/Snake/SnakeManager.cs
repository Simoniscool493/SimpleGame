using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games.FoodEatingGame.Payloads;
using System.Threading;

namespace SimpleGame.Games.Snake
{
    class SnakeManager : IDiscreteGameManager
    {
        public DiscreteIOInfo IOInfo { get; }

        public IDiscreteGameIOAdapter IOADapter => new SnakeIOAdapter();

        public IDiscreteGameStateProvider StateProvider => new SnakeStateProvider();

        public SnakeManager()
        {
            IOInfo = new DiscreteIOInfo
            (
                inputInfo: new SnakeInputInfo(),
                outputInfo: new SnakeOutputInfo()
            );
        }

        public void Demonstrate(IDiscreteDecider decider, IDiscreteGameState state)
        {
            Run(decider, (SnakeState)state, false);
        }

        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            return Run(decider, (SnakeState)state, true);
        }

        public int Play()
        {
            var playState = ((SnakeStateProvider)StateProvider).GetStateForPlay();

            while (true)
            {
                playState.Tick();

                Thread.Sleep(50);
            }
        }


        private int Run(IDiscreteDecider decider, SnakeState state,bool headless)
        {
            while (true)
            {
                var status = IOADapter.GetOutput(state);

                if (status.Data[0] == SnakeConstants.GameOver)
                {
                    state.Dispose();
                    return status.Data[1] + (status.Data[2] * 255);
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state, direction);
                state.Tick();

                if (!headless)
                {
                    Thread.Sleep(3);//3
                }
            }
        }
    }
}
