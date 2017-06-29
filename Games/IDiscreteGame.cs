using SimpleGame.DataPayloads;
using SimpleGame.Games.FoodEatingGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games
{
    interface IDiscreteGame
    {
        DiscreteIOInfo IOInfo { get; }

        int Score(IDiscreteDecider decider,IGameState state);

        void Demonstrate(IDiscreteDecider decider, IGameState state);

        IGameState GetNextStateForTraining();
    }
}
