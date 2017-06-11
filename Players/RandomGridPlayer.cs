using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Players
{
    class RandomGridPlayer : IGridPlayer
    {
        Random r = new Random();

        public Direction GetDirection(ItemAtPoint[] upDownLeftright)
        {
            return (Direction)r.Next(0, 4);
        }
    }
}
