using SimpleGame.Deciders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.Games.FoodEatingGame;
using SimpleGame.Games;
using SimpleGame.DataPayloads;

namespace SimpleGame.Players
{
    class AiGridPlayer
    {
        public IDiscreteDecider Decider;

        public AiGridPlayer(IDiscreteDecider decider)
        {
            Decider = decider;
        }

        public Direction GetDirection(int[] upDownLeftright)
        {
            var payload = new DiscreteDataPayload(typeof(ItemAtPoint), upDownLeftright);
            return (Direction)Decider.Decide(payload).SingleItem;
        }

    }
}
