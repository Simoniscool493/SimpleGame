using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.Snake
{
    class SnakeStateProvider : IDiscreteGameStateProvider
    {
        public IDiscreteGameState GetStateForDemonstration(int randomSeed)
        {
            return new SnakeState(false,randomSeed);
        }

        public IDiscreteGameState GetStateForTraining(int randomSeed)
        {
            return new SnakeState(true,randomSeed);
        }

        public SnakeState GetStateForPlay()
        {
            return new SnakeState(false,new Random().Next(),true);
        }
    }
}
