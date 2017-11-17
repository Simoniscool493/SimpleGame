using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.Iris
{
    class IrisStateProvider : IDiscreteGameStateProvider
    {
        public int RandomSeed { get; set; }

        public IDiscreteGameState GetStateForDemonstration()
        {
            throw new NotImplementedException();
        }

        public IDiscreteGameState GetStateForNextGeneration()
        {
            return new IrisDataInstance(new Random(RandomSeed));
        }
    }
}
