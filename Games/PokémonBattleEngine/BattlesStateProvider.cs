using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.PokémonBattleEngine
{
    class BattlesStateProvider : IDiscreteGameStateProvider
    {
        public BattlesState state;

        public BattlesStateProvider()
        {
            this.state = new BattlesState();
        }

        public IDiscreteGameState GetStateForDemonstration(int randomSeed)
        {
            state.SetRandomSeed(randomSeed);
            return state;
        }

        public IDiscreteGameState GetStateForTraining(int randomSeed)
        {
            state.SetRandomSeed(randomSeed);
            return state;
        }

    }
}
