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
    public class GameBoard
    {
        public PictureBox BoardImage = ActualPacmanGameInstance.IS_HEADLESS ? null : new PictureBox();
        public const int matrixWidth = 28;
        public const int matrixHeight = 31;

        public int[,] Matrix = new int[matrixHeight-1, matrixWidth-1];
        public int[] HorizCounts = new int[matrixHeight];
        public int[] VertCounts = new int[matrixWidth];

        public GameBoard()
        {
            InitialiseBoardMatrix(1);
        }

        public void CreateBoardImage(Form formInstance, int Level)
        {
            // Create Board Image
            BoardImage.Name = "BoardImage";
            BoardImage.SizeMode = PictureBoxSizeMode.AutoSize;
            BoardImage.Location = new Point(0, 50);
            switch (Level)
            {
                case 1: BoardImage.Image = Properties.Resources.Board_1; break;
            }
            formInstance.Controls.Add(BoardImage);
        }

        public void DecrementPelletCounts(int x,int y)
        {
            HorizCounts[x]--;
            VertCounts[y]--;

            if(HorizCounts[x] <0 || VertCounts[y]<0)
            {
                throw new Exception();
            }
        }

        public int[] GetPelletDirections(int x,int y)
        {
            int areTherePelletsAbove = 0;
            int areTherePelletsBelow = 0;
            int areTherePelletsOnMyY = 0;

            int areTherePelletsToRight = 0;
            int areTherePelletsToLeft = 0;
            int areTherePelletsOnMyX = 0;

            for (int i=0;i<y;i++)
            {
                if(HorizCounts[i]!=0)
                {
                    areTherePelletsAbove = 1;
                }
            }

            if(HorizCounts[y]!=0)
            {
                areTherePelletsOnMyY = 1;
            }

            for (int i=(y+1); i<matrixHeight;i++)
            {
                if (HorizCounts[i] != 0)
                {
                    areTherePelletsBelow = 1;
                }
            }

            for (int i = 0; i < x; i++)
            {
                if (VertCounts[i] != 0)
                {
                    areTherePelletsToLeft= 1;
                }
            }

            if (HorizCounts[x] != 0)
            {
                areTherePelletsOnMyX = 1;
            }

            for (int i = (x + 1); i < matrixWidth; i++)
            {
                if (VertCounts[i] != 0)
                {
                    areTherePelletsToRight = 1;
                }
            }

            return new[] { areTherePelletsAbove, areTherePelletsBelow, areTherePelletsToLeft, areTherePelletsToRight };
            //return new[] { areTherePelletsAbove,areTherePelletsBelow,areTherePelletsOnMyY,areTherePelletsToLeft,areTherePelletsToRight,areTherePelletsOnMyX};
        }

        public Tuple<int,int> InitialiseBoardMatrix(int Level)
        {
            // Initialise Game Board Matrix
            switch (Level)
            {
                case 1:
                    Matrix = new int[,] {
                        { 10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10 },
                        { 10,01,01,01,01,01,01,01,01,01,01,01,01,10,10,01,01,01,01,01,01,01,01,01,01,01,01,10 },
                        { 10,01,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,01,10 },
                        { 10,02,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,02,10 },
                        { 10,01,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,01,10 },
                        { 10,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,10 },
                        { 10,01,10,10,10,10,01,10,10,01,10,10,10,10,10,10,10,10,01,10,10,01,10,10,10,10,01,10 },
                        { 10,01,10,10,10,10,01,10,10,01,10,10,10,10,10,10,10,10,01,10,10,01,10,10,10,10,01,10 },
                        { 10,01,01,01,01,01,01,10,10,01,01,01,01,10,10,01,01,01,01,10,10,01,01,01,01,01,01,10 },
                        { 10,10,10,10,10,10,01,10,10,10,10,10,00,10,10,00,10,10,10,10,10,01,10,10,10,10,10,10 },
                        { 10,10,10,10,10,10,01,10,10,10,10,10,00,10,10,00,10,10,10,10,10,01,10,10,10,10,10,10 },
                        { 10,10,10,10,10,10,01,10,10,00,00,00,00,00,00,00,00,00,00,10,10,01,10,10,10,10,10,10 },
                        { 10,10,10,10,10,10,01,10,10,00,10,10,10,11,11,10,10,10,00,10,10,01,10,10,10,10,10,10 },
                        { 10,10,10,10,10,10,01,10,10,00,10,10,10,15,15,10,10,10,00,10,10,01,10,10,10,10,10,10 },
                        { 00,00,00,00,00,00,01,00,00,00,10,10,10,15,15,10,10,10,00,00,00,01,00,00,00,00,00,00 },
                        { 10,10,10,10,10,10,01,10,10,00,10,10,10,10,10,10,10,10,00,10,10,01,10,10,10,10,10,10 },
                        { 10,10,10,10,10,10,01,10,10,00,10,10,10,10,10,10,10,10,00,10,10,01,10,10,10,10,10,10 },
                        { 10,10,10,10,10,10,01,10,10,03,00,00,00,00,00,00,00,00,00,10,10,01,10,10,10,10,10,10 },
                        { 10,10,10,10,10,10,01,10,10,00,10,10,10,10,10,10,10,10,00,10,10,01,10,10,10,10,10,10 },
                        { 10,10,10,10,10,10,01,10,10,00,10,10,10,10,10,10,10,10,00,10,10,01,10,10,10,10,10,10 },
                        { 10,01,01,01,01,01,01,01,01,01,01,01,01,10,10,01,01,01,01,01,01,01,01,01,01,01,01,10 },
                        { 10,01,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,01,10 },
                        { 10,01,10,10,10,10,01,10,10,10,10,10,01,10,10,01,10,10,10,10,10,01,10,10,10,10,01,10 },
                        { 10,02,01,01,10,10,01,01,01,01,01,01,01,00,00,01,01,01,01,01,01,01,10,10,01,01,02,10 },
                        { 10,10,10,01,10,10,01,10,10,01,10,10,10,10,10,10,10,10,01,10,10,01,10,10,01,10,10,10 },
                        { 10,10,10,01,10,10,01,10,10,01,10,10,10,10,10,10,10,10,01,10,10,01,10,10,01,10,10,10 },
                        { 10,01,01,01,01,01,01,10,10,01,01,01,01,10,10,01,01,01,01,10,10,01,01,01,01,01,01,10 },
                        { 10,01,10,10,10,10,10,10,10,10,10,10,01,10,10,01,10,10,10,10,10,10,10,10,10,10,01,10 },
                        { 10,01,10,10,10,10,10,10,10,10,10,10,01,10,10,01,10,10,10,10,10,10,10,10,10,10,01,10 },
                        { 10,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,01,10 },
                        { 10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10,10 } };

                    for(int h=0;h<HorizCounts.Length; h++)
                    {
                        for(int h2=0;h2<matrixWidth;h2++)
                        {
                            if(Matrix[h,h2]==1 || Matrix[h,h2]==2)
                            {
                                HorizCounts[h]++;
                            }
                        }
                    }

                    for (int v = 0; v < VertCounts.Length; v++)
                    {
                        for (int v2 = 0; v2 < matrixHeight; v2++)
                        {
                            if (Matrix[v2, v] == 1 || Matrix[v2, v] == 2)
                            {
                                VertCounts[v]++;
                            }
                        }
                    }

                    break;
            }
            int StartX = 0;
            int StartY = 0;
            for (int y=0; y<30; y++)
            {
                for (int x=0; x<27; x++)
                {
                    if (Matrix[y, x] == 3) { StartX = x; StartY = y;}
                }
            }
            Tuple<int,int> StartLocation = new Tuple<int,int> (StartX, StartY);
            return StartLocation;
        }
    }
}
