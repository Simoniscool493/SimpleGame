using System;
using Battles;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleGame.DataPayloads.DiscreteData;
using SimpleGame.Deciders.Discrete;
using SimpleGame.Games.PokémonBattleEngine.Controller;
using SimpleGame.Games.PokémonBattleEngine.IOInfo;

namespace SimpleGame.Games.PokémonBattleEngine
{
    class BattlesGameManager : IDiscreteGameManager
    {
        public DiscreteIOInfo IOInfo { get; }

        public IDiscreteGameIOAdapter IOADapter => throw new NotImplementedException();
        public IDiscreteGameStateProvider StateProvider => _stateProvider;
        private IDiscreteGameStateProvider _stateProvider = new BattlesStateProvider();

        public BattlesGameManager()
        {
            IOInfo = new DiscreteIOInfo(inputInfo: new BattlesInputInfo(), outputInfo: new BattlesOutputInfo());
        }

        public void Demonstrate(IDiscreteDecider decider, IDiscreteGameState state)
        {
            var gameState = (BattlesState)state;
            gameState.state.TestRunBattle(new AIBattleController(decider));
        }

        public int Score(IDiscreteDecider decider, IDiscreteGameState state)
        {
            var gameState = (BattlesState)state;
            return gameState.state.TestRunBattle(new AIBattleController(decider))[0];
        }
    }
}
