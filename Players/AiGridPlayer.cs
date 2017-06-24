using SimpleGame.Deciders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.Games.FoodEatingGame;

namespace SimpleGame.Players
{
    class AiGridPlayer : IGridPlayer
    {
        public IDecider Decider;

        public AiGridPlayer(IDecider decider)
        {
            Decider = decider;
        }

        public Direction GetDirection(ItemAtPoint[] upDownLeftright)
        {
            return Decider.GetDirection(upDownLeftright);
        }
    }
}
