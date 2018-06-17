using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Pacman
{
    public class HighScore
    {
        public int InitalScore;
        public Label HighScoreText = ActualPacmanGameInstance.IS_HEADLESS ? null : new Label();
        public int Score;

        public HighScore(int initial)
        {
            InitalScore = initial;
            Score = initial;

            if(!ActualPacmanGameInstance.IS_HEADLESS)
            {
                HighScoreText.Text = Score.ToString();
            }
        }

        public void CreateHighScore(Form formInstance)
        {
            // Create Score label
            HighScoreText.ForeColor = System.Drawing.Color.White;
            HighScoreText.Font = new System.Drawing.Font("Folio XBd BT", 14);
            HighScoreText.Top = 23;
            HighScoreText.Left = 170;
            HighScoreText.Height = 20;
            HighScoreText.Width = 100;
            formInstance.Controls.Add(HighScoreText);
            HighScoreText.BringToFront();
            UpdateHighScore(InitalScore);
        }

        public void UpdateHighScore(int value)
        {
            Score = value;
            if (!ActualPacmanGameInstance.IS_HEADLESS)
            {
                //HighScoreText.Text = Score.ToString();
            }
        }

    }
}
