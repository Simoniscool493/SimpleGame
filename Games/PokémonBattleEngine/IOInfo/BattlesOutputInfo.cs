using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.PokémonBattleEngine.IOInfo
{
    [Serializable()]
    class BattlesOutputInfo : DiscreteDataPayloadInfo
    {
        public BattlesOutputInfo()
        {
            valuePoints.Add(new ValuePoint(new int[] { 1, 2, 3, 4, 5 }));
        }
    }
}
