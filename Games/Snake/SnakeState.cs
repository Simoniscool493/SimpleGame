using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleGraphics;

namespace SimpleGame.Games.Snake
{
    class SnakeState : IDiscreteGameState
    {
        private SnakeProgram _instance;
        private int _randomSeed;
        
        public SnakeState(bool isHeadless,int randomSeed,bool isPlayerMode = false)
        {
            _instance = new SnakeProgram();
            _instance.Setup(isHeadless,!isPlayerMode,randomSeed);

            _randomSeed = randomSeed;
        }

        public void Dispose()
        {
            
        }

        public void Reset()
        {
            _instance.Setup(_instance.IsHeadless,true,_randomSeed);
        }

        public int[] GetStatus()
        {
            return _instance.GetStatus();
        }

        public void SendInput(int d)
        {
             _instance.SendInput(d);
        }

        public void Tick()
        {
            _instance.Tick();
        }
    }
}
