using Pacman;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Games.FoodEatingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.FoodEatingGame.Payloads
{
    [Serializable()]
    class FoodEatingGameInputInfo : DiscreteDataPayloadInfo
    {
        public FoodEatingGameInputInfo()
        {
            for(int i=0;i< 25;i++)
            {
                valuePoints.Add(new ValuePoint(typeof(ItemAtPoint)));
            }

            this.dataPointNames = new string[] { "Top", "Bottom", "Left", "Right", "IsThereFood" };
        }
    }
}
