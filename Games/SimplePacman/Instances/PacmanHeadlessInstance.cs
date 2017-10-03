using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pacman;

namespace SimpleGame.Games.SimplePacman
{
    class PacmanHeadlessInstance : IPacmanInstance
    {
        public PacmanHeadlessInstance()
        {
            ActualPacmanGameInstance.SetUpInstance(true);
        }

        public void SendInput(Direction d)
        {
            ActualPacmanGameInstance.Instance.pacman.moveQueue.Enqueue((int)(d+1));
        }

        public int[] GetStatus()
        {
            return ActualPacmanGameInstance.Instance.GetStatus();
        }

        public void Tick()
        {
            ActualPacmanGameInstance.Instance.Tick();
        }

        public void Dispose()
        {

        }

        public void Reset()
        {
            ActualPacmanGameInstance.SetUpInstance(true);
        }

    }
}
