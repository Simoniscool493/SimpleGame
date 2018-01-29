using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.Snake
{
    class SnakeStateProvider : IDiscreteGameStateProvider
    {
        public int RandomSeed { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IDiscreteGameState GetStateForDemonstration()
        {
            return new SnakeState(false);
        }

        public IDiscreteGameState GetStateForNextGeneration()
        {
            return new SnakeState(true);
        }
    }
}
