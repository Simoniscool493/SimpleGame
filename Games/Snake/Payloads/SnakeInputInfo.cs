using Pacman;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Games.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.FoodEatingGame.Payloads
{
    [Serializable()]
    class SnakeInputInfo : DiscreteDataPayloadInfo
    {
        public SnakeInputInfo()
        {
            for (int i = 0; i < 8; i++)
            {
                valuePoints.Add(new ValuePoint(typeof(SnakePointData)));
            }

            for (int i = 0; i < 2; i++)
            {
                valuePoints.Add(new ValuePoint(new[] { 0, 1, 2 }));
            }
        }
    }
}
