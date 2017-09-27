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
    class PacmanInstance : IDiscreteGameState
    {
        Process pacmanProcess;

        NamedPipeServerStream stream;
        Random r = new Random();

        public PacmanInstance()
        {
            Reset();
        }

        public void Reset()
        {
            Dispose();

            pacmanProcess = new Process();
            pacmanProcess.StartInfo = new ProcessStartInfo(typeof(PacmanLauncher).Assembly.Location);
            pacmanProcess.Start();

            stream = new NamedPipeServerStream("PacmanPipe", PipeDirection.InOut);
            stream.WaitForConnection();
        }

        public void Dispose()
        {
            stream?.Close();
            pacmanProcess?.Close();
        }

        public void SendInput(Direction d)
        {
            byte[] toSend = { (byte)d };

            stream.Write(toSend, 0, 1);
        }

        public int[] GetStatus()
        {
            stream.Write(new byte[] { PacmanConstants.STATUS_REQUEST }, 0, 1);

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
