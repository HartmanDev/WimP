using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PathFinder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //int X, Y;
        private bool Precis = false;
        public int SelectedMap = 0;
        private byte PosXDest;
        private byte PosYDest;
        private byte PosXMob;
        private byte PosYMob;
        private bool Loaded = false;
        private FindPath FP;
        // ----- MAP ----- //
        private List<int[,]> ListeDesMaps = new List<int[,]>();
        private int[,] tmaptemp;
        private int[,] tmap0 = 
        {
           
            {0,0,0,1,0,0,0,0,1,0,1,0,0,1,0,0},
            {0,1,0,0,1,1,0,1,0,0,0,1,0,0,1,0},
            {0,0,1,0,0,0,0,0,1,0,1,0,1,0,0,0},
            {1,1,0,0,1,0,1,0,0,0,1,0,0,1,0,0},
            {0,0,0,1,0,0,0,1,0,0,1,0,1,0,0,1},
            {0,1,0,0,0,1,0,0,0,1,0,0,0,0,1,0},
            {0,0,1,1,1,1,0,1,0,0,0,1,0,1,1,0},
            {0,0,1,0,0,0,0,1,0,0,1,0,0,0,0,0},
            {1,0,0,1,0,1,0,0,0,1,0,0,1,1,1,0},
            {0,1,0,0,0,0,0,1,1,0,0,1,1,0,0,0},
            {0,0,0,0,1,1,0,0,0,1,0,1,0,0,1,0},
            {1,0,1,1,0,0,1,1,0,0,0,0,0,1,0,0},
            {0,0,0,0,1,0,0,0,0,1,0,0,1,0,0,1},
            {0,1,0,1,0,0,0,1,0,0,0,1,0,0,1,0},
            {1,0,0,0,0,1,0,0,0,1,0,0,1,0,0,0},
            {0,0,1,0,0,0,0,1,1,0,0,0,0,0,1,0}
        };
        private int[,] tmap1 = 
        {
           
            {0,1,0,1,0,0,0,0,0},
            {0,0,0,1,1,1,0,0,0},
            {1,1,0,0,0,0,0,0,0},
            {1,0,0,1,0,0,1,0,0},
            {0,0,1,1,1,0,0,0,0},
            {0,1,1,1,0,0,0,0,0},
            {1,0,0,1,0,1,0,1,1},
            {0,0,1,1,1,0,0,0,0},
            {0,1,1,1,0,0,1,1,0}
        };
        private int[,] tmap2 = 
        {
           
            {1,0,0,0,0,0,1,0,0},
            {0,0,1,0,1,0,0,0,0},
            {1,0,0,0,0,0,1,1,0},
            {0,0,0,0,1,0,0,0,0},
            {0,0,1,0,1,0,1,0,1},
            {1,1,0,0,0,0,1,0,0},
            {0,0,0,1,0,0,0,0,0},
            {0,1,1,1,0,1,0,1,0},
            {0,0,0,0,0,1,0,0,0}
        };
        private int[,] Path;
        int[] MapDim = new int[2];
        // ----- GRAPHICS ----- //
        private const int Echelle = 25;
        private void Form1_Load(object sender, EventArgs e)
        {
            // Charge les maps
            if (!Loaded)
            {
                ListeDesMaps.Add(tmap0);
                ListeDesMaps.Add(tmap1);
                ListeDesMaps.Add(tmap2);
                Loaded = true;
            }
            MapDim[0] = ListeDesMaps[SelectedMap].GetLength(0);
            MapDim[1] = ListeDesMaps[SelectedMap].GetLength(1);
            DrawMap();
        }

        private void DrawMap()
        {
            //
            panel1.Refresh();
            
            // Update
            MapDim[0] = ListeDesMaps[SelectedMap].GetLength(0);
            MapDim[1] = ListeDesMaps[SelectedMap].GetLength(1);
            // 
            tbTabX.Text = MapDim[0].ToString();
            tbTabY.Text = MapDim[1].ToString();
            // Chargement de la map
            tmaptemp = new int[MapDim[0], MapDim[1]];
            Array.Copy(ListeDesMaps[SelectedMap], tmaptemp, MapDim[0] * MapDim[1]);
            // --------------
            Graphics g = panel1.CreateGraphics();
            Brush brush = new SolidBrush(Color.Black);
            // --------------
            for (int i = 0; i < tmaptemp.GetLength(0); i++)
            {
                for (int j = 0; j < tmaptemp.GetLength(1); j++)
                {
                    if (tmaptemp[i,j] == 1)
                    {
                        brush = new SolidBrush(Color.Black);
                        g.FillRectangle(brush, j * Echelle, i * Echelle, Echelle, Echelle);
                    }
                    else
                    {
                        brush = new SolidBrush(Color.White);
                        g.FillRectangle(brush, j * Echelle, i * Echelle, Echelle, Echelle);
                    }
                }
            }
            for (int i = 0; i < tmaptemp.GetLength(0); i++)
            {
                brush = new SolidBrush(Color.DarkViolet);
                g.DrawString(i.ToString(), new Font("Arial", 12f), brush, new PointF(i * Echelle,0));
            }
            for (int j = 0; j < tmaptemp.GetLength(1); j++)
            {
                brush = new SolidBrush(Color.Orange);
                g.DrawString(j.ToString(), new Font("Arial", 12f), brush, new PointF(0, j * Echelle));
            }
            
        }
        private void DrawPath()
        {
            MapDim[0] = ListeDesMaps[SelectedMap].GetLength(0);
            MapDim[1] = ListeDesMaps[SelectedMap].GetLength(1);
            DrawMap();
            Graphics g = panel1.CreateGraphics();
            Brush brush = new SolidBrush(Color.Aqua);
            for (int i = 0; i < MapDim[0]; i++ )
            {
                for (int j = 0; j < MapDim[1]; j++)
                {
                    // --- Dessin
                    if (Path[i,j] != 0)
                        g.FillRectangle(brush, j * Echelle, i * Echelle, Echelle, Echelle);
                }
            }
            //----Destination
            brush = new SolidBrush(Color.Red);
            g.FillRectangle(brush, PosYDest * Echelle, PosXDest * Echelle, Echelle, Echelle);
            //----Départ
            brush = new SolidBrush(Color.Green);
            g.FillRectangle(brush, PosYMob * Echelle, PosXMob * Echelle, Echelle, Echelle);
        }

        private void SearchPath_Click(object sender, EventArgs e)
        {
            tbxErrorLog.Text = "";
            try
            {
                FP = new FindPath();
                PosXDest = Convert.ToByte(DestX.Text);
                PosYDest = Convert.ToByte(DestY.Text);

                PosXMob = Convert.ToByte(ObjX.Text);
                PosYMob = Convert.ToByte(ObjY.Text);

                TimeSpan TS;
                DateTime dt = DateTime.Now;

                Path = FP.Find(tmaptemp, PosXMob, PosYMob, PosXDest, PosYDest, Precis);
                TS = DateTime.Now - dt;
                DrawPath();
                lblInfoTime.Text = "Temps requis : " + TS;
            }
            catch (Exception ex)
            {
                tbxErrorLog.Text = ex.Message;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DrawMap();
        }

        private void btnDEBUG_Click(object sender, EventArgs e)
        {
            if (lblDEBUG.Text == "Etat actuel : OFF")
            {
                FP.DEBUG = true;
                lblDEBUG.Text = "Etat actuel : ON";
            }
            else
            {
                FP.DEBUG = false;
                lblDEBUG.Text = "Etat actuel : OFF";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void InstanceChoixMap(object sender, EventArgs e)
        {
            Form2 Form2 = new Form2(this);
            Form2.LoadMap(ListeDesMaps);
            Form2.Show();
        }
        // --- --- --- --- ---
        private void button3_Click(object sender, EventArgs e)
        {
            // RECUP DANS FICHIER LE DEBUG
            string text = System.IO.File.ReadAllText(@".\WiMP_Logs.txt");
            MessageBoxScrollable m = new MessageBoxScrollable(text);
            m.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ObjX.Text = "";
            ObjY.Text = "";
            DestX.Text = "";
            DestY.Text = "";
            tbxErrorLog.Text = "";
            lblInfoTime.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (lblPrecis.Text == "Précision : normale")
            {
                Precis = true;
                lblPrecis.Text = "Précision : double";
            }
            else
            {
                lblPrecis.Text = "Précision : normale";
                Precis = false;
            }
        }
    }
}
