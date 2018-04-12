using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Pipes;
using System.Threading;

//Pacman C# Implementation created by Adrian Eyre. Available at https://github.com/adrianeyre/pacman

namespace Pacman
{
    public static class PacmanLauncher
    {
        public static bool IS_PIPED = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            if(args.Contains("pipedInstance"))
            {
                IS_PIPED = true;

                StartGameWithPipe();
            }

            if (IS_PIPED)
            {
                StartGameWithPipe();
            }
            else
            {
                StartGame();
            }
        }

        public static void StartGameWithPipe()
        {
            stream = new NamedPipeClientStream("PacmanPipe");
            stream.Connect();
            new TaskFactory().StartNew(() => InputLoop());

            StartGame();
        }

        public static void StartGame()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            instance = new PacmanScreen();
            keyDownEvent = typeof(PacmanScreen).GetMethod("OnKeyDown",BindingFlags.Instance | BindingFlags.NonPublic);
            Application.Run(instance);
        }

        public static void InputLoop()
        {
            while (instance == null) { Thread.Sleep(50); }

            var status = ActualPacmanGameInstance.Instance.GetStatus();
            SendStatus(status);

            while (stream.IsConnected)
            {
                byte[] input = new byte[1];
                stream.Read(input,0,1);

                ActualPacmanGameInstance.Instance.SendInput(input[0]);
                status = ActualPacmanGameInstance.Instance.GetStatus();
                SendStatus(status);
            }
        }

        public static void SendStatus(int[] status)
        {
            byte[] toSend = status.Select(n => (byte)n).ToArray();

            stream.Write(toSend, 0, ActualPacmanGameInstance.LengthOfDataToSend);
        }

        static int counter = 25;

        public static NamedPipeClientStream stream;
        public static PacmanScreen instance;
        public static MethodInfo keyDownEvent;
    }
}
