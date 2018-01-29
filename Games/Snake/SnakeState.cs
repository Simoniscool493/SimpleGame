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
        private ConsoleGraphics.Program _instance;
        
        public SnakeState(bool isHeadless)
        {
            _instance = new ConsoleGraphics.Program();
            _instance.Setup(isHeadless,true);
        }

        public void Dispose()
        {
            
        }

        public void Reset()
        {
            _instance.Setup(_instance.IsHeadless,true);
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
