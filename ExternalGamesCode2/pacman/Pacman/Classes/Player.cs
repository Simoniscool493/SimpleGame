using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Pacman
{
    public class Player
    {
        private const int MaxLives = 5;
        public int Score { get; set; }
        public int Lives = ActualPacmanGameInstance.NUMLIVES;
        public Label ScoreText = ActualPacmanGameInstance.IS_HEADLESS ? null : new Label();
        public PictureBox[] LifeImage = new PictureBox[MaxLives];

        public void CreateLives(Form formInstance)
        {
            for(int x = 0; x < MaxLives; x++)
            {
                LifeImage[x] = new PictureBox();
                LifeImage[x].Name = "Life" + x.ToString();
                LifeImage[x].SizeMode = PictureBoxSizeMode.AutoSize;
                LifeImage[x].Location = new Point(x * 30 + 3, 550);
                LifeImage[x].Image = Properties.Resources.Life;
                formInstance.Controls.Add(LifeImage[x]);
                LifeImage[x].BringToFront();
            }
            SetLives();
        }

        public void CreatePlayerDetails(Form formInstance)
        {
            // Create Score label
            ScoreText.ForeColor = System.Drawing.Color.White;
            ScoreText.Font = new System.Drawing.Font("Folio XBd BT", 14);
            ScoreText.Top = 23;
            ScoreText.Left = 30;
            ScoreText.Height = 20;
            ScoreText.Width = 100;
            formInstance.Controls.Add(ScoreText);
            ScoreText.BringToFront();
            UpdateScore(0);
        }

        public void UpdateScore(int amount = 1)
        {
            if(amount<0 && Score<ActualPacmanGameInstance.DEATH_PENALTY)
            {
                Score = 0;
                return;
            }

            ActualPacmanGameInstance.NoScoreCount = 0;
            //ActualPacmanGameInstance.Log($"Score increased by {amount}. Previous: {Score}. New: {Score + amount}");
            Score += amount;

            // Update score value and text
            if (!ActualPacmanGameInstance.IS_HEADLESS)
            {
                ScoreText.Text = Score.ToString();
            }

            if (Score > ActualPacmanGameInstance.Instance.highscore.Score) { ActualPacmanGameInstance.Instance.highscore.UpdateHighScore(Score); }
        }

        public void SetLives()
        {
            if(!ActualPacmanGameInstance.IS_HEADLESS)
            {
                // Display lives in form
                for (int x = 0; x < Lives + 1; x++)
                {
                    LifeImage[x].Visible = true;
                }
                for (int x = Lives - 1; x < MaxLives; x++)
                {
                    LifeImage[x].Visible = false;
                }
            }
        }

        public void LoseLife()
        {
            if(ActualPacmanGameInstance.DEATH_PENALTY!=0)
            {
                ActualPacmanGameInstance.Instance.player.UpdateScore(-ActualPacmanGameInstance.DEATH_PENALTY);
            }

            // Lose a life
            Lives--;
            if (Lives > 0)
            {
                ActualPacmanGameInstance.Instance.pacman.Set_Pacman();
                ActualPacmanGameInstance.Instance.ghost.ResetGhosts();
                //GHOSTDELETEDHERE

                SetLives();
            }
            else
            {
                ActualPacmanGameInstance.EndGame();
            }
        }

        public void LevelComplete()
        {
            ActualPacmanGameInstance.EndGame();
        }
    }
}
