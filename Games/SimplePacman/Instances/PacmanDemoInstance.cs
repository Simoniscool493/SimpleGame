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
         
        private Process pacmanProcess;
        private bool isHookedIn;
        private int _randomSeed;

        public PacmanDemoInstance(bool attachToExistingInstance,int randomSeed)
        {
            if(stream==null)
            {
                stream = new NamedPipeServerStream("PacmanPipe", PipeDirection.InOut);
            }

            isHookedIn = attachToExistingInstance;
            _randomSeed = randomSeed;

            Reset();
        }

        public void Reset()
        {
            Dispose();

            if(!isHookedIn)
            {
                pacmanProcess = new Process();
                pacmanProcess.StartInfo = new ProcessStartInfo(typeof(PacmanLauncher).Assembly.Location);
                pacmanProcess.StartInfo.Arguments = "pipedInstance " + _randomSeed;
                pacmanProcess.Start();
            }

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
            byte[] toSend = { (byte)(d+1) };

            try
            {
                stream.Write(toSend, 0, 1);
            }
            catch (IOException)
            {
                Console.WriteLine("Pacman Demo Instance termimated. Press enter to quit.");
                Console.ReadLine();
                Environment.Exit(0);
            }
        }

        public int[] GetStatus()
        {
            //stream.Write(new byte[] { PacmanConstants.STATUS_REQUEST }, 0, 1);

            byte[] status = new byte[ActualPacmanGameInstance.LengthOfDataToSend];
            stream.Read(status, 0, ActualPacmanGameInstance.LengthOfDataToSend);

            return status.Select(b => (int)b).ToArray();
        }
    }
}
