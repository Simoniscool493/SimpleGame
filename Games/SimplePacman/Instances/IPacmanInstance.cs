using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SimplePacman
{
    interface IPacmanInstance : IDiscreteGameState
    {
        void SendInput(Direction d);

        int[] GetStatus();
    }
}
