using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SpaceInvaders.Instances
{
    class SpaceInvadersHeadlessInstance : ISpaceInvadersInstance
    {
        CalceranosInvaders.MainForm _instance;

        public SpaceInvadersHeadlessInstance()
        {
            Reset();
        }

        public void Dispose()
        {
            _instance = null;
        }

        public int[] GetStatus()
        {
            return _instance.GetStatus();
        }

        public void Reset()
        {
            _instance = new CalceranosInvaders.MainForm(true,false);
            _instance.InitializeObjects();
        }

        public void SendInput(int d)
        {
            _instance.SendInput(d);
        }
    }
}
