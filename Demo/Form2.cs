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
    public partial class Form2 : Form
    {
        private Form1 frm;
        public Form2()
        {
            InitializeComponent();
        }

        public Form2(Form1 frmUn)
        {
            InitializeComponent();
            frm = frmUn;
        }
       
        private const int Echelle = 15;
        private short nbMap = 0;
        private Bitmap bm;
        private int CaseX = 0;
        private int CaseY = 0;
        private int WidthTab = 0;
        private int HeightTab = 0;
        public string strnamebutton = "";

        private List<Button> listbutton = new List<Button>();
        public void LoadMap(List<int[,]> ListeDesMaps)
        {
            
            List<PictureBox> listmap = new List<PictureBox>();
            foreach (int[,] map in ListeDesMaps)
            {
                
                CaseX = 0;
                Button Bu = new Button();
                CaseY = 0;
                PictureBox pB = new PictureBox();
                bm = new Bitmap(map.GetLength(0) * Echelle, map.GetLength(1) * Echelle);
                pB.Size = bm.Size;
                
                // --------------
                if (nbMap > 0)
                {
                    WidthTab = listmap[nbMap - 1].Width + 10;
                    HeightTab = listmap[nbMap - 1].Height + 10;
                }

                foreach (int Value in map)
                {
                    for (int X = CaseX * Echelle; X < (CaseX * Echelle) + Echelle; X++)
                    {
                        for (int Y = CaseY * Echelle; Y < (CaseY * Echelle) + Echelle; Y++)
                        {
                            if (map[CaseX, CaseY] == 1)
                                bm.SetPixel(Y, X, Color.Black);
                            else
                                bm.SetPixel(Y, X, Color.White);
                        }
                    }
                    CaseY++;
                    if (CaseY == map.GetLength(1))
                    {
                        CaseY = 0;
                        CaseX++;
                    }
                }
                pB.Location = new Point(WidthTab * nbMap, 0);
                pB.Image = bm;
                pB.Name = "Map" + nbMap.ToString();
                listmap.Add(pB);
                //this.Controls.Add(listmap[nbMap]);

                if (nbMap == 0)
                    Bu.Location = new Point(10, 10);
                else
                    Bu.Location = new Point(listbutton[nbMap - 1].Width + 10 + listbutton[nbMap - 1].Location.X, 10);
                Bu.Visible = true;
                Bu.Name = nbMap.ToString();
                Bu.Image = bm;
                Bu.Width = pB.Width;
                Bu.Height = pB.Height;
                Bu.Text = "Labyrinthe " + nbMap.ToString();
                Bu.Font = new Font("Arial", 15.0f);
                Bu.ForeColor = Color.Orange;
                listbutton.Add(Bu);
                
                nbMap++;  
            }

            foreach (Button b in listbutton) 
            {
                b.Click += new System.EventHandler(b_selectmap);
                this.Controls.Add(b);
            }
        }
        private void b_selectmap(object sender, EventArgs e)
        {
            frm.SelectedMap = Convert.ToInt32(((Button)sender).Name);
            this.Close();
        }
    }
}
