using System;

namespace SimpleGame.Games
{
    public interface IDiscreteGameState : IDisposable
    { 
        void Reset();
    }
}
