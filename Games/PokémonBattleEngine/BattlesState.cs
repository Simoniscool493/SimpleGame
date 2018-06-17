using Battles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.PokémonBattleEngine
{
    class BattlesState : IDiscreteGameState
    {
        public SimpleGameInterface state;

        public BattlesState()
        {
            state = new SimpleGameInterface();
            state.Load();
        }

        public void SetRandomSeed(int randomSeed)
        {
            state.SetRandomSeed(randomSeed);
        }
        
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public void Reset()
        {
            //throw new NotImplementedException();
        }
    }
}
