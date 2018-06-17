using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SpaceInvaders.Instances
{
    class SpaceInvadersHeadlessInstance : ISpaceInvadersInstance
    {
        private CalceranosInvaders.MainForm _instance;
        private int _randomSeed;

        public SpaceInvadersHeadlessInstance(int randomSeed)
        {
            _randomSeed = randomSeed;
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
            _instance = new CalceranosInvaders.MainForm(true,false,_randomSeed);
            _instance.InitializeObjects();
        }

        public void SendInput(int d)
        {
            _instance.SendInput(d);
        }
    }
}
