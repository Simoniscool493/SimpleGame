using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGame.Games.SpaceInvaders.Instances
{
    class SpaceInvadersDemoInstance : ISpaceInvadersInstance
    {
        private static NamedPipeServerStream stream;

        private Process invadersProcess;
        private bool isHookedIn;

        public SpaceInvadersDemoInstance(bool attachToExistingInstance)
        {
            if (stream == null)
            {
                stream = new NamedPipeServerStream("SpaceInvadersPipe", PipeDirection.InOut);
            }

            isHookedIn = attachToExistingInstance;

            Reset();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int[] GetStatus()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            Dispose();

            if (!isHookedIn)
            {
                invadersProcess = new Process();
                invadersProcess.StartInfo = new ProcessStartInfo(typeof(CalceranosInvaders.Program).Assembly.Location);
                invadersProcess.StartInfo.Arguments = "pipedInstance";
                invadersProcess.Start();
            }

            stream.WaitForConnection();
        }

        public void SendInput(int d)
        {
            throw new NotImplementedException();
        }
    }
}
