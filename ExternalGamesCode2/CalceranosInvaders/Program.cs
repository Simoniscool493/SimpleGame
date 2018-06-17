/*
 * Creato da SharpDevelop.
 * Utente: Giovanni Calcerano
 * Data: 24/05/2008
 * Ora: 17.35
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

/*

 Space Invaders C# Implementation created by Giovanni Calcerano. Available at https://www.codeproject.com/Articles/27036/C-Space-Invaders-using-WinForms-Objects

*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;

namespace CalceranosInvaders
{
	/// <summary>
	/// Class with program entry point.
	/// </summary>
	public sealed class Program
	{
        public static bool isPiped = true;
		/// <summary>
		/// Program entry point.
		/// </summary>
		[STAThread]
		private static void Main(string[] args)
		{
            var isPiped = (args.Length>0) && (args[0].Equals("pipedInstance"));

            var randomSeed = int.Parse(args.Last());
            //isPiped = true;

            Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm(false, isPiped, randomSeed));
		}
	}
}
