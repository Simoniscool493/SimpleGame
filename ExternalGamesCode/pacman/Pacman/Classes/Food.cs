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
    public class Food
    {
        public PictureBox[,] FoodImage = new PictureBox[30,27];
        public int Amount = 0;


        public void InitializeFood()
        {
            for (int y = 0; y < 30; y++)
            {
                for (int x = 0; x < 27; x++)
                {
                    if (ActualPacmanGameInstance.Instance.gameboard.Matrix[y, x] == 1)
                    {
                        Amount++;
                    }
                }
            }
        }

        public void CreateFoodImages(Form formInstance)
        {
            for (int y = 0; y < 30; y++)
            {
                for (int x = 0; x < 27; x++)
                {
                    if (ActualPacmanGameInstance.Instance.gameboard.Matrix[y,x] == 1 || ActualPacmanGameInstance.Instance.gameboard.Matrix[y, x] == 2)
                    {
                        FoodImage[y, x] = new PictureBox();
                        FoodImage[y, x].Name = "FoodImage" + Amount.ToString();
                        FoodImage[y, x].SizeMode = PictureBoxSizeMode.AutoSize;
                        FoodImage[y, x].Location = new Point(x * 16 - 1, y * 16 + 47);
                        if (ActualPacmanGameInstance.Instance.gameboard.Matrix[y,x] == 1)
                        {
                            FoodImage[y, x].Image = Properties.Resources.Block_1;
                            Amount++;
                        }
                        else
                        {
                            FoodImage[y, x].Image = Properties.Resources.Block_2;
                        }
                        formInstance.Controls.Add(FoodImage[y, x]);
                        FoodImage[y, x].BringToFront();
                        
                    }
                }
            }
        }

        public void EatFood(int x, int y)
        {
            /*lock(ActualPacmanGameInstance.Instance)
            {
                ActualPacmanGameInstance.Instance.gameboard.DecrementPelletCounts(x, y);
            }*/

            // Eat food
            if (!ActualPacmanGameInstance.IS_HEADLESS)
            {
                FoodImage[x, y].Visible = false;
            }

            ActualPacmanGameInstance.Instance.gameboard.Matrix[x, y] = 0;
            ActualPacmanGameInstance.Instance.player.UpdateScore(ActualPacmanGameInstance.FOOD_SCORE);
            Amount--;
            if (Amount < 1) { ActualPacmanGameInstance.Instance.player.LevelComplete(); }
            //Form1.audio.Play(1);
        }

        public void EatSuperFood(int x, int y)
        {
            /*lock (ActualPacmanGameInstance.Instance)
            {
                ActualPacmanGameInstance.Instance.gameboard.DecrementPelletCounts(x, y);
            }*/

            // Eat food
            if (!ActualPacmanGameInstance.IS_HEADLESS)
            {
                FoodImage[x, y].Visible = false;
            }
            ActualPacmanGameInstance.Instance.gameboard.Matrix[x, y] = 0;
            ActualPacmanGameInstance.Instance.player.UpdateScore(ActualPacmanGameInstance.SUPER_FOOD_SCORE);
            ActualPacmanGameInstance.Instance.ghost.ChangeGhostState();
            //GHOSTDELETEDHERE
        }
    }
}
