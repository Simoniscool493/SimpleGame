using Battles.BattleEngine;
using SimpleGame.DataPayloads.DiscreteData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.PokémonBattleEngine.IOInfo
{
    [Serializable()]
    class BattlesInputInfo : DiscreteDataPayloadInfo
    {
        public BattlesInputInfo()
        {
            valuePoints.Add(new ValuePoint(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })); //Foe HP (binned)
            valuePoints.Add(new ValuePoint(typeof(Battles.ElementalType)));               //Foe type1
            //valuePoints.Add(new ValuePoint(typeof(Battles.ElementalType)));               //Foe type2
            valuePoints.Add(new ValuePoint(typeof(Battles.ElementalType)));               //Move type
            valuePoints.Add(new ValuePoint(new int[] { 1, 2, 3, 4, 5 }));               //Move power (binned)
            valuePoints.Add(new ValuePoint(new int[] { 1, 2, 3, 4, 5 }));               //Move acccuracy (binned)
            valuePoints.Add(new ValuePoint(typeof(MoveClass)));                        //Move class
        }
    }
}
