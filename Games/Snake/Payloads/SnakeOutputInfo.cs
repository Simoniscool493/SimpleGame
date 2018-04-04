using Pacman;
using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.FoodEatingGame.Payloads
{
    [Serializable()]
    class SnakeOutputInfo : DiscreteDataPayloadInfo
    {
        public SnakeOutputInfo()
        {
            valuePoints.Add(new ValuePoint(typeof(Direction)));
        }
    }
}
