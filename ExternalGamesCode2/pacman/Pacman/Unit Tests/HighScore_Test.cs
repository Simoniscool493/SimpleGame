using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Pacman
{
    [TestFixture]
    public class HighScore_Test
    {
        public HighScore HighScore = new HighScore(100);

        [Test]
        public void CreateHighScoreTest()
        {
            // Check High Score has been created
            HighScore.CreateHighScore(new Form());
            Assert.AreEqual(HighScore.InitalScore.ToString(), HighScore.HighScoreText.Text);
        }
    }
}
