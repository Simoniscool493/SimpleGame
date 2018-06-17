using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.Iris
{
    class IrisStateProvider : IDiscreteGameStateProvider
    {
        public IDiscreteGameState GetStateForDemonstration(int randomSeed)
        {
            throw new NotImplementedException();
        }

        public IDiscreteGameState GetStateForTraining(int randomSeed)
        {
            return new IrisDataInstance(new Random(randomSeed));
        }
    }
}
