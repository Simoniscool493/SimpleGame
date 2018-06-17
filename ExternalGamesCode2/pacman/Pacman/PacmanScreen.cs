using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacman
{
    public partial class PacmanScreen : Form
    {
        //public static Audio audio = new Audio();
        private static FormElements formelements = new FormElements();

        public PacmanScreen(int randomSeed)
        {
            ActualPacmanGameInstance.SetUpInstance(false,randomSeed);

            Thread.Sleep(3000);

            InitializeComponent();
            SetupGame(1);
        }

        public void SetupGame(int Level)
        {
            // Create Game Board
            ActualPacmanGameInstance.Instance.gameboard.CreateBoardImage(this, Level);

            // Create Board Matrix
            Tuple<int, int> PacmanStartCoordinates = ActualPacmanGameInstance.Instance.gameboard.InitialiseBoardMatrix(Level);

            // Create Player
            ActualPacmanGameInstance.Instance.player.CreatePlayerDetails(this);
            ActualPacmanGameInstance.Instance.player.CreateLives(this);

            // Create Form Elements
            formelements.CreateFormElements(this);

            // Create High Score
            ActualPacmanGameInstance.Instance.highscore.CreateHighScore(this);

            // Create Food
            ActualPacmanGameInstance.Instance.food.CreateFoodImages(this);

            // Create Ghosts
            ActualPacmanGameInstance.Instance.ghost.CreateGhostImage(this);
            //GHOSTDELETEDHERE

            // Create Pacman
            ActualPacmanGameInstance.Instance.pacman.CreatePacmanImage(this, PacmanStartCoordinates.Item1, PacmanStartCoordinates.Item2);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            switch (e.KeyCode)
            {
                case Keys.Up: ActualPacmanGameInstance.Instance.pacman.nextDirection = 1; ActualPacmanGameInstance.Instance.pacman.MovePacman(1); break;
                case Keys.Right: ActualPacmanGameInstance.Instance.pacman.nextDirection = 2; ActualPacmanGameInstance.Instance.pacman.MovePacman(2); break;
                case Keys.Down: ActualPacmanGameInstance.Instance.pacman.nextDirection = 3; ActualPacmanGameInstance.Instance.pacman.MovePacman(3); break;
                case Keys.Left: ActualPacmanGameInstance.Instance.pacman.nextDirection = 4; ActualPacmanGameInstance.Instance.pacman.MovePacman(4); break;
            }
        }

    }
}
