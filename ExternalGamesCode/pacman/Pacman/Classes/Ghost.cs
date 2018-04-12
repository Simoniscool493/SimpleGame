using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Pacman
{
    public class Ghost
    {
        public static Random ran = new Random();
        private const int GhostTimeScared = 50;
        private const int GhostSpeedFactor = 1;
        private const int GhostAmount = 4;

        public int Ghosts = GhostAmount;
        private ImageList GhostImages = new ImageList();
        public PictureBox[] GhostImage = new PictureBox[GhostAmount];
        public int[] State = new int[GhostAmount];
        public int[] xCoordinate = new int[GhostAmount];
        public int[] yCoordinate = new int[GhostAmount];
        private int[] xStart = new int[GhostAmount];
        private int[] yStart = new int[GhostAmount];
        public int[] Direction = new int[GhostAmount];
        private bool GhostOn = false;
        private int ScaredTimer = 0;

        public Ghost()
        {
            if(ActualPacmanGameInstance.IS_DETERMINISTIC)
            {
                ran = new Random(ActualPacmanGameInstance.RANDOM_SEED);
            }

            if (!ActualPacmanGameInstance.IS_HEADLESS)
            {
                GhostImages.Images.Add(Properties.Resources.Ghost_0_1);
                GhostImages.Images.Add(Properties.Resources.Ghost_0_2);
                GhostImages.Images.Add(Properties.Resources.Ghost_0_3);
                GhostImages.Images.Add(Properties.Resources.Ghost_0_4);

                GhostImages.Images.Add(Properties.Resources.Ghost_1_1);
                GhostImages.Images.Add(Properties.Resources.Ghost_1_2);
                GhostImages.Images.Add(Properties.Resources.Ghost_1_3);
                GhostImages.Images.Add(Properties.Resources.Ghost_1_4);

                GhostImages.Images.Add(Properties.Resources.Ghost_2_1);
                GhostImages.Images.Add(Properties.Resources.Ghost_2_2);
                GhostImages.Images.Add(Properties.Resources.Ghost_2_3);
                GhostImages.Images.Add(Properties.Resources.Ghost_2_4);

                GhostImages.Images.Add(Properties.Resources.Ghost_3_1);
                GhostImages.Images.Add(Properties.Resources.Ghost_3_2);
                GhostImages.Images.Add(Properties.Resources.Ghost_3_3);
                GhostImages.Images.Add(Properties.Resources.Ghost_3_4);

                GhostImages.Images.Add(Properties.Resources.Ghost_4);
                GhostImages.Images.Add(Properties.Resources.Ghost_5);

                GhostImages.ImageSize = new Size(27, 28);
            }

            Set_Ghosts();

            if(ActualPacmanGameInstance.IS_HEADLESS)
            {
                ResetGhosts();
            }
        }

        public void CreateGhostImage(Form formInstance)
        {
            // Create Ghost Image
            for (int x=0; x<Ghosts; x++)
            {
                GhostImage[x] = new PictureBox();
                GhostImage[x].Name = "GhostImage" + x.ToString();
                GhostImage[x].SizeMode = PictureBoxSizeMode.AutoSize;
                formInstance.Controls.Add(GhostImage[x]);
                GhostImage[x].BringToFront();
            }
            Set_Ghosts();
            ResetGhosts();
        }

        public void Set_Ghosts()
        {
            xStart[0] = 13;
            yStart[0] = 13;

            xStart[1] = 14;
            yStart[1] = 13;

            xStart[2] = 13;
            yStart[2] = 14;

            xStart[3] = 14;
            yStart[3] = 14;
        }

        public void ResetGhosts()
        {
            // Reset Ghost States
            for (int x=0; x<GhostAmount; x++)
            {
                xCoordinate[x] = xStart[x];
                yCoordinate[x] = yStart[x];

                if(!ActualPacmanGameInstance.IS_HEADLESS)
                {
                    GhostImage[x].Location = new Point(xStart[x] * 16 - 3, yStart[x] * 16 + 43);
                    GhostImage[x].Image = GhostImages.Images[x * 4];
                }

                Direction[x] = 0;
                State[x] = 0;
            }
        }

        public void Tick()
        {
            MainTick();
            StateTick();
            HomeTick();
            KillableTick();
        }

        private void StateTick()
        {
            if(ScaredTimer>0)
            {
                ScaredTimer--;
                if(ScaredTimer==0)
                {
                    for (int x = 0; x < GhostAmount; x++)
                    {
                        State[x] = 0;
                    }
                }
            }
        }

        private void HomeTick()
        {
            // Move ghosts to their home positions
            for (int x=0; x<GhostAmount; x++)
            {
                if (State[x] == 2)
                {
                    /*int xpos = xStart[x] * 16 - 3;
                    int ypos = yStart[x] * 16 + 43;
                    if(!ActualPacmanGameInstance.IS_HEADLESS)
                    {
                        if (GhostImage[x].Left > xpos) { GhostImage[x].Left--; }
                        if (GhostImage[x].Left < xpos) { GhostImage[x].Left++; }
                        if (GhostImage[x].Top > ypos) { GhostImage[x].Top--; }
                        if (GhostImage[x].Top < ypos) { GhostImage[x].Top++; }
                        if (GhostImage[x].Top == ypos && GhostImage[x].Left == xpos)
                        {
                            State[x] = 0;
                            xCoordinate[x] = xStart[x];
                            yCoordinate[x] = yStart[x];
                            GhostImage[x].Left = xStart[x] * 16 - 3;
                            GhostImage[x].Top = yStart[x] * 16 + 43;
                        }
                    }
                    else
                    {
                            State[x] = 0;
                            xCoordinate[x] = xStart[x];
                            yCoordinate[x] = yStart[x];
                    }*/

                    State[x] = 0;
                    xCoordinate[x] = xStart[x];
                    yCoordinate[x] = yStart[x];

                    if(!ActualPacmanGameInstance.IS_HEADLESS)
                    {
                        GhostImage[x].Left = xStart[x] * 16 - 3;
                        GhostImage[x].Top = yStart[x] * 16 + 43;
                    }
                }
            }
        }

        private void MainTick()
        {
            // Keep moving the ghosts
            for (int x = 0; x < Ghosts; x++)
            {
                if (State[x] > 0) { continue; }
                MoveGhosts(x);
            }
            GhostOn = !GhostOn;
            CheckForPacman();
        }

        private void KillableTick()
        {
            // Keep moving the ghosts
            for (int x = 0; x < Ghosts; x++)
            {
                if (State[x] != 1) { continue; }
                MoveGhosts(x);
            }
        }

        private void MoveGhosts(int x)
        {
            // Move the ghosts
            if (Direction[x] == 0)
            {
                if (ran.Next(0, 5) == 3) { Direction[x] = 1;}
            }
            else
            {
                bool CanMove = false;
                Other_Direction(Direction[x], x);

                while (!CanMove)
                {
                    CanMove = check_direction(Direction[x], x);
                    if (!CanMove) { Change_Direction(Direction[x], x); }

                }

                if (CanMove)
                {
                    switch (Direction[x])
                    {
                        case 1: if (!ActualPacmanGameInstance.IS_HEADLESS) { GhostImage[x].Top -= 16; } yCoordinate[x]--; break;
                        case 2: if (!ActualPacmanGameInstance.IS_HEADLESS) { GhostImage[x].Left += 16; } xCoordinate[x]++; break;
                        case 3: if (!ActualPacmanGameInstance.IS_HEADLESS) { GhostImage[x].Top += 16; } yCoordinate[x]++; break;
                        case 4: if (!ActualPacmanGameInstance.IS_HEADLESS) { GhostImage[x].Left -= 16; } xCoordinate[x]--; break;
                    }

                    if (!ActualPacmanGameInstance.IS_HEADLESS)
                    {
                        switch (State[x])
                        {
                            case 0: GhostImage[x].Image = GhostImages.Images[x * 4 + (Direction[x] - 1)]; break;
                            case 1:
                                if (GhostOn) { GhostImage[x].Image = GhostImages.Images[17]; } else { GhostImage[x].Image = GhostImages.Images[16]; };
                                break;
                            case 2: GhostImage[x].Image = GhostImages.Images[18]; break;
                        }
                    }
                }
            }
            
        }

        private bool check_direction(int direction, int ghost)
        {
            // Check if ghost can move to space
            switch (direction)
            {
                case 1: return direction_ok(xCoordinate[ghost], yCoordinate[ghost] - 1, ghost);
                case 2: return direction_ok(xCoordinate[ghost] + 1, yCoordinate[ghost], ghost);
                case 3: return direction_ok(xCoordinate[ghost], yCoordinate[ghost] + 1, ghost);
                case 4: return direction_ok(xCoordinate[ghost] - 1, yCoordinate[ghost], ghost);
                default: return false;
            }
        }

        private bool direction_ok(int x, int y, int ghost)
        {
            // Check if board space can be used
            if (x < 0) { xCoordinate[ghost] = 27; if (!ActualPacmanGameInstance.IS_HEADLESS) { GhostImage[ghost].Left = 429; }  return true; }
            if (x > 27) { xCoordinate[ghost] = 0; if (!ActualPacmanGameInstance.IS_HEADLESS) { GhostImage[ghost].Left = -5; } return true; }
            if (ActualPacmanGameInstance.Instance.gameboard.Matrix[y, x] < 4 || ActualPacmanGameInstance.Instance.gameboard.Matrix[y, x] > 10) { return true; } else { return false; }
        }

        private void Change_Direction(int direction, int ghost)
        {
            // Change the direction of the ghost
            int which = ran.Next(0, 2);
            switch (direction)
            {
                case 1: case 3: if (which == 1) { Direction[ghost] = 2; } else { Direction[ghost] = 4; }; break;
                case 2: case 4: if (which == 1) { Direction[ghost] = 1; } else { Direction[ghost] = 3; }; break;
            }
        }

        private void Other_Direction(int direction, int ghost)
        {
            // Check to see if the ghost can move a different direction
            if (ActualPacmanGameInstance.Instance.gameboard.Matrix[yCoordinate[ghost], xCoordinate[ghost]] < 4)
            {
                bool[] directions = new bool[5];
                int x = xCoordinate[ghost];
                int y = yCoordinate[ghost];
                switch (direction)
                {
                    case 1: case 3: directions[2] = direction_ok(x + 1, y, ghost); directions[4] = direction_ok(x - 1, y, ghost); break;
                    case 2: case 4: directions[1] = direction_ok(x, y - 1, ghost); directions[3] = direction_ok(x, y + 1, ghost); break;
                }
                int which = ran.Next(0, 5);
                if (directions[which] == true) { Direction[ghost] = which; }
            }
        }

        public void ChangeGhostState()
        {
            ScaredTimer = GhostTimeScared;

            // Change the state off all of the ghosts so that they can be eaten
            for (int x=0; x<GhostAmount; x++)
            {
                if (State[x] == 0)
                {
                    State[x] = 1;
                    if(!ActualPacmanGameInstance.IS_HEADLESS)
                    {
                        GhostImage[x].Image = GhostImages.Images[16];
                    }
                }
            }
        }

        public void CheckForPacman()
        {
            // Check to see if a ghost is on the same block as Pacman
            for (int x = 0; x < GhostAmount; x++)
            {
                ///TEMP

                ///Temp
                if (xCoordinate[x] == ActualPacmanGameInstance.Instance.pacman.xCoordinate && yCoordinate[x] == ActualPacmanGameInstance.Instance.pacman.yCoordinate)
                {
                    ActualPacmanGameInstance.Log("Ghost at pacman position. State = " + State[x]);

                    switch (State[x])
                    {
                        case 0: if (ActualPacmanGameInstance.ARE_GHOSTS_ACTIVE) { ActualPacmanGameInstance.Instance.player.LoseLife(); } break;
                        case 1:
                            State[x] = 2;
                            if(!ActualPacmanGameInstance.IS_HEADLESS)
                            {
                                GhostImage[x].Image = Properties.Resources.eyes;
                            }
                            ActualPacmanGameInstance.Instance.player.UpdateScore(ActualPacmanGameInstance.GHOST_EATING_SCORE);
                            break;
                    }
                }
            }
        }
    }
}
