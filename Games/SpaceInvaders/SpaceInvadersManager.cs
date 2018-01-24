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
using System.IO;

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
            StringBuilder sb = null;

            if (ShouldLog)
            {
                sb = new StringBuilder("Demonstration:");
            }

            int steps = 0;

            while (true)
            {
                var status = IOADapter.GetOutput(state);

                if (status.Data[0] == SpaceInvadersConstants.GAME_OVER)
                {
                    if (ShouldLog)
                    {
                        var score = status.Data[1] + (status.Data[2] * 255);
                        sb.AppendLine("Game over with score: " + score);

                        using (StreamWriter writer = new StreamWriter("C:\\ProjectLogs\\demo.txt"))
                        {
                            writer.Write(sb.ToString());
                        }
                    }

                    state.Dispose();
                    return;
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state, direction);

                if (ShouldLog)
                {
                    sb.AppendLine(string.Join(", ", status) + " => " + direction);
                }

                if (!(state is SpaceInvadersHeadlessInstance))
                {
                    Thread.Sleep(50);
                }

                if (steps++ > 20000)
                {
                    throw new Exception();
                }
            }
        }

        public static bool ShouldLog = false;

        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            StringBuilder sb = null;

            if (ShouldLog)
            {
                sb = new StringBuilder("Headless:");
            }

            state.Reset();
            int steps = 0;

            while (true)
            {
                var status = IOADapter.GetOutput(state);

                if (status.Data[0] == SpaceInvadersConstants.GAME_OVER)
                {
                    var score = status.Data[1] + (status.Data[2] * 255);

                    if (ShouldLog)
                    {
                        sb.AppendLine("Game over with score: " + score);

                        using (StreamWriter writer = new StreamWriter("C:\\ProjectLogs\\headless.txt"))
                        {
                            writer.Write(sb.ToString());
                        }
                    }

                    state.Dispose();
                    return score;
                }

                var direction = decider.Decide(status);
                IOADapter.SendInput(state, direction);

                if(ShouldLog)
                {
                    sb.AppendLine(string.Join(", ", status) + " => " + direction);
                }

                if (!(state is SpaceInvadersHeadlessInstance))
                {
                    Thread.Sleep(75);
                }

                if(steps++>20000)
                {
                    throw new Exception();
                }
            }
        }
    }
}
