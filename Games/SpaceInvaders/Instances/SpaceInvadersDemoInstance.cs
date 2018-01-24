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
            if (stream.IsConnected)
            {
                stream?.Disconnect();
            }

            invadersProcess?.Kill();
        }

        public int[] GetStatus()
        {
            byte[] status = new byte[SpaceInvadersConstants.LengthOfStatus];
            stream.Read(status, 0, SpaceInvadersConstants.LengthOfStatus);

            return status.Select(b => (int)b).ToArray();
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

            Console.WriteLine("Waiting for connection..");
            stream.WaitForConnection();
            Console.WriteLine("Got connection.");

        }

        public void SendInput(int d)
        {
            byte[] toSend = { (byte)(d) };

            stream.Write(toSend, 0, 1);
        }
    }
}
