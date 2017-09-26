using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pacman;
using System.Diagnostics;
using System.IO.Pipes;
using System.IO;

namespace SimpleGame.Games.SimplePacman
{
    class PacmanInstance : IDiscreteGameState
    {
        PacmanScreen instance;

        public PacmanInstance()
        {
            var process = new Process();
            Process.Start(new ProcessStartInfo(typeof(PacmanLauncher).Assembly.Location));


            var pipeServer = new NamedPipeServerStream("PacmanPipe", PipeDirection.InOut);
            pipeServer.WaitForConnection();
            Console.ReadLine();

            byte[] toSend = { 33 };
            pipeServer.Write(toSend, 0, 1);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}
