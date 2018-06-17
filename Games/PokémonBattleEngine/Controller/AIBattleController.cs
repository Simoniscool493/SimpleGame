using System;
using Battles.BattleEngine.Battle;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Battles;
using Battles.BattleEngine;
using SimpleGame.Deciders.Discrete;
using SimpleGame.DataPayloads.DiscreteData;

namespace SimpleGame.Games.PokémonBattleEngine.Controller
{
    class AIBattleController : IBattleController
    {
        public IDiscreteDecider decider;

        public AIBattleController(IDiscreteDecider decider)
        {
            this.decider = decider;
        }

        public Move GetMove(BattleInstance self, BattleInstance foe)
        {
            if(self.NoPPLeft)
            {
                return Move.Struggle;
            }

            var priorityDict = new Dictionary<Move, int>();

            var foeHpToTen = foe.BattleStats.HpPercentageInt * 10;
            var foeType1 = (int)(foe.BattleInstanceType1);
            //var foeType2 = (int)(foe.BattleInstanceType1);

           
            foreach(Move m in self.Moves.Where(m=>self.PowerPoints[m]>0))
            {
                var moveType = (int)(m.MoveType);
                var movePower = (int)(m.Power / (double)Move.MaxMovePower) * 5;
                var moveAccuracy = (int)((m.Accuracy / 100d) * 5);
                var moveClass = (int)m.MoveClass;

                //var inputForThisMove = new DiscreteDataPayload(new int[] { foeHpToTen,foeType1,foeType2,moveType,movePower,moveAccuracy, moveClass });
                var inputForThisMove = new DiscreteDataPayload(new int[] { foeHpToTen, foeType1, moveType, movePower, moveAccuracy, moveClass });

                priorityDict[m] = decider.Decide(inputForThisMove).SingleItem; 
            }

            var choice = priorityDict.FirstOrDefault(kvp => kvp.Value == priorityDict.Max(kv=>kv.Value)).Key;
            self.PowerPoints[choice]--;

            return choice;
        }
    }
}
