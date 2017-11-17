using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;

namespace SimpleGame.Games.Iris
{
    class IrisManager : IDiscreteGameManager
    {
        public DiscreteIOInfo IOInfo { get; }

        public IDiscreteGameIOAdapter IOADapter { get; } = new IrisIOAdapter();

        public IDiscreteGameStateProvider StateProvider { get; } = new IrisStateProvider();

        public IrisManager()
        {
            IOInfo = new DiscreteIOInfo
            (
                inputInfo: new DiscretedContinuousDataPayloadInfo(3,10),
                outputInfo: new DiscreteDataPayloadInfo(typeof(IrisSpecies), 1, new string[] { "output" })
            );
        }

        public void Demonstrate(IDiscreteDecider decider, IDiscreteGameState state)
        {
            throw new NotImplementedException();
        }

        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            int score = 0;
            var theState = (IrisDataInstance)state;

            foreach(var point in theState.evalData)
            {
                var i1 = (int)(point.Feature1 * 10);
                var i2 = (int)(point.Feature2 * 10);
                var i3 = (int)(point.Feature3 * 10);
                var i4 = (int)(point.Feature4 * 10);

                int[] input = { i1, i2, i3, i4 };

                var decision = decider.Decide(new DiscretedContinuousDataPayload(input));

                if(decision.SingleItem == (int)(point.Species))
                {
                    score++;
                }
            }


            return score;
        }
    }
}
