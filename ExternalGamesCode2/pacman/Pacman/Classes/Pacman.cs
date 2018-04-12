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
    public class Pacman
    {
        // Initialise variables
        public int xCoordinate = 0;
        public int yCoordinate = 0;
        private int xStart = 9;
        private int yStart = 17;
        public int currentDirection = 0;
        public int nextDirection = 0;
        public PictureBox PacmanImage = ActualPacmanGameInstance.IS_HEADLESS ?  null : new PictureBox();
        private ImageList PacmanImages = ActualPacmanGameInstance.IS_HEADLESS ? null : new ImageList();
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        private List<Tuple<int, int>> previousLocations = new List<Tuple<int, int>>();

        private int imageOn = 0;

        public Pacman()
        {
            if(!ActualPacmanGameInstance.IS_HEADLESS)
            {
                PacmanImages.Images.Add(Properties.Resources.Pacman_1_0);
                PacmanImages.Images.Add(Properties.Resources.Pacman_1_1);
                PacmanImages.Images.Add(Properties.Resources.Pacman_1_2);
                PacmanImages.Images.Add(Properties.Resources.Pacman_1_3);

                PacmanImages.Images.Add(Properties.Resources.Pacman_2_0);
                PacmanImages.Images.Add(Properties.Resources.Pacman_2_1);
                PacmanImages.Images.Add(Properties.Resources.Pacman_2_2);
                PacmanImages.Images.Add(Properties.Resources.Pacman_2_3);

                PacmanImages.Images.Add(Properties.Resources.Pacman_3_0);
                PacmanImages.Images.Add(Properties.Resources.Pacman_3_1);
                PacmanImages.Images.Add(Properties.Resources.Pacman_3_2);
                PacmanImages.Images.Add(Properties.Resources.Pacman_3_3);

                PacmanImages.Images.Add(Properties.Resources.Pacman_4_0);
                PacmanImages.Images.Add(Properties.Resources.Pacman_4_1);
                PacmanImages.Images.Add(Properties.Resources.Pacman_4_2);
                PacmanImages.Images.Add(Properties.Resources.Pacman_4_3);

                PacmanImages.ImageSize = new Size(27, 28);
            }


            Set_Pacman();
        }

        public void CreatePacmanImage(Form formInstance, int StartXCoordinate, int StartYCoordinate)
        {
            // Create Pacman Image
            xStart = StartXCoordinate;
            yStart = StartYCoordinate;
            PacmanImage.Name = "PacmanImage";
            PacmanImage.SizeMode = PictureBoxSizeMode.AutoSize;
            Set_Pacman();
            formInstance.Controls.Add(PacmanImage);
            PacmanImage.BringToFront();
        }

        public void MovePacman(int direction)
        {
            // Move Pacman
            bool CanMove = check_direction(nextDirection);
            if (!CanMove) { CanMove = check_direction(currentDirection); direction = currentDirection; } else { direction = nextDirection; }
            if (CanMove) { currentDirection = direction; }

            if (CanMove)
            {
                switch (direction)
                {
                    case 1: if (!ActualPacmanGameInstance.IS_HEADLESS) { PacmanImage.Top -= 16; } yCoordinate--; break;
                    case 2: if (!ActualPacmanGameInstance.IS_HEADLESS) { PacmanImage.Left += 16; } xCoordinate++; break;
                    case 3: if (!ActualPacmanGameInstance.IS_HEADLESS) { PacmanImage.Top += 16; }; yCoordinate++; break;
                    case 4: if (!ActualPacmanGameInstance.IS_HEADLESS) { PacmanImage.Left -= 16; }  xCoordinate--; break;
                }
                currentDirection = direction;
                UpdatePacmanImage();
                CheckPacmanPosition();

                if(ActualPacmanGameInstance.NEW_POSITION_SCORE!=0 || ActualPacmanGameInstance.OLD_POSITION_PENALTY != 0)
                {
                    var currentPosition = new Tuple<int, int>(xCoordinate, yCoordinate);

                    if(!previousLocations.Contains(currentPosition))
                    {
                        if(ActualPacmanGameInstance.NEW_POSITION_SCORE != 0)
                        {
                            ActualPacmanGameInstance.Instance.player.UpdateScore(ActualPacmanGameInstance.NEW_POSITION_SCORE);
                        }
                        previousLocations.Add(currentPosition);
                    }
                    else
                    {
                        if (ActualPacmanGameInstance.OLD_POSITION_PENALTY != 0)
                        {
                            ActualPacmanGameInstance.Instance.player.UpdateScore(-ActualPacmanGameInstance.OLD_POSITION_PENALTY);
                        }
                    }
                }
                //GHOSTDELETEDHERE
            }
            else if(ActualPacmanGameInstance.WALL_BUMPING_PENALTY !=0)
            {
                ActualPacmanGameInstance.Instance.player.UpdateScore(-ActualPacmanGameInstance.WALL_BUMPING_PENALTY);
            }

            ActualPacmanGameInstance.Instance.ghost.CheckForPacman();
        }

        private void CheckPacmanPosition()
        {
            // Check Pacmans position
            switch (ActualPacmanGameInstance.Instance.gameboard.Matrix[yCoordinate, xCoordinate])
            {
                case 1: ActualPacmanGameInstance.Instance.food.EatFood(yCoordinate, xCoordinate); break;
                case 2: ActualPacmanGameInstance.Instance.food.EatSuperFood(yCoordinate, xCoordinate); break;
            }
        }

        public void UpdatePacmanImage()
        {
            if(!ActualPacmanGameInstance.IS_HEADLESS)
            {
                // Update Pacman image
                PacmanImage.Image = PacmanImages.Images[((currentDirection - 1) * 4) + imageOn];
                imageOn++;
                if (imageOn > 3) { imageOn = 0; }
            }
        }

        private bool check_direction(int direction)
        {
            // Check if pacman can move to space
            switch (direction)
            {
                case 1: return direction_ok(xCoordinate, yCoordinate - 1);
                case 2: return direction_ok(xCoordinate + 1, yCoordinate);
                case 3: return direction_ok(xCoordinate, yCoordinate + 1);
                case 4: return direction_ok(xCoordinate - 1, yCoordinate);
                default: return false;
            }
        }

        private bool direction_ok(int x, int y)
        {
            // Check if board space can be used
            if (x < 0) { xCoordinate = 27; if (!ActualPacmanGameInstance.IS_HEADLESS) { PacmanImage.Left = 429; } return true ; }
            if (x > 27) { xCoordinate = 0; if (!ActualPacmanGameInstance.IS_HEADLESS) { PacmanImage.Left = -5; } return true; }
            if (ActualPacmanGameInstance.Instance.gameboard.Matrix[y, x] < 4) { return true; } else { return false; }
        }

        public void Tick()
        {
            if(ActualPacmanGameInstance.IS_HEADLESS)
            {
                if (moveQueue.Any())
                {
                    var direction = moveQueue.Dequeue();
                    if (direction == 1000)
                    {
                        return;
                    }

                    nextDirection = direction;
                    MovePacman(direction);
                }
            }
            else
            {
                lock (moveLock)
                {
                    if (moveQueue.Any())
                    {
                        var direction = moveQueue.Dequeue();
                        if (direction == 1000)
                        {
                            return;
                        }

                        nextDirection = direction;
                        MovePacman(direction);
                    }
                }
            }


            // Keep moving pacman
            MovePacman(currentDirection);
        }

        public Queue<int> moveQueue = new Queue<int>();

        public void Set_Pacman()
        {
            // Place Pacman in board
            currentDirection = 0;
            nextDirection = 0;
            xCoordinate = xStart;
            yCoordinate = yStart;

            if (!ActualPacmanGameInstance.IS_HEADLESS)
            {
                PacmanImage.Image = Properties.Resources.Pacman_2_1;
                PacmanImage.Location = new Point(xStart * 16 - 3, yStart * 16 + 43);
            }
        }

        public static object moveLock = new object();
    }
}
