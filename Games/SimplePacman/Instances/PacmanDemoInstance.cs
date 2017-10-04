using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pacman;
using System.Diagnostics;
using System.IO.Pipes;
using System.IO;
using System.Timers;

namespace SimpleGame.Games.SimplePacman
{
    class PacmanDemoInstance : IPacmanInstance
    {
        private static NamedPipeServerStream stream;
         
        private static Random r = new Random();
        private Process pacmanProcess;
        private bool isDemonstration;

        public PacmanDemoInstance()
        {
            if(stream==null)
            {
                stream = new NamedPipeServerStream("PacmanPipe", PipeDirection.InOut);
            }

            Reset();
        }

        public void Reset()
        {
            Dispose();

            pacmanProcess = new Process();
            pacmanProcess.StartInfo = new ProcessStartInfo(typeof(PacmanLauncher).Assembly.Location);
            if(!isDemonstration)
            {
                pacmanProcess.StartInfo.Arguments = "testingMode";
            }
            pacmanProcess.Start();

            stream.WaitForConnection();
        }

        public void Dispose()
        {
            if(stream.IsConnected)
            {
                stream?.Disconnect();
            }
            pacmanProcess?.Kill();
        }

        public void SendInput(Direction d)
        {
            byte[] toSend = { (byte)d };

            stream.Write(toSend, 0, 1);
        }

        public int[] GetStatus()
        {
            //stream.Write(new byte[] { PacmanConstants.STATUS_REQUEST }, 0, 1);

            byte[] status = new byte[8];
            stream.Read(status, 0, 8);

            return status.Select(b => (int)b).ToArray();
        }

        private void SendRandomInput(object sender, ElapsedEventArgs e)
        {
            byte[] toSend = { (byte)r.Next(1, 5) };

            stream.Write(toSend, 0, 1);
        }

    }
}
