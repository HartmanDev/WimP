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
    public partial class MessageBoxScrollable : Form
    {
        public MessageBoxScrollable()
        {
            InitializeComponent();
        }

        public MessageBoxScrollable(string strText)
        {
            InitializeComponent();
            this.Name = "Fichier logs";
            this.Text = "Fichier logs";
            textBox1.Text = strText;
            this.Focus();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
