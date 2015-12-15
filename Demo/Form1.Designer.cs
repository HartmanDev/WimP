namespace PathFinder
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.ObjX = new System.Windows.Forms.TextBox();
            this.ObjY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DestY = new System.Windows.Forms.TextBox();
            this.DestX = new System.Windows.Forms.TextBox();
            this.SearchPath = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTabY = new System.Windows.Forms.TextBox();
            this.tbTabX = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lblInfoTime = new System.Windows.Forms.Label();
            this.btnDEBUG = new System.Windows.Forms.Button();
            this.lblDEBUG = new System.Windows.Forms.Label();
            this.tbxErrorLog = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.lblY = new System.Windows.Forms.Label();
            this.lblX = new System.Windows.Forms.Label();
            this.btnLoadmapChoice = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.lblPrecis = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(30, 82);
            this.panel1.MaximumSize = new System.Drawing.Size(480, 480);
            this.panel1.MinimumSize = new System.Drawing.Size(480, 480);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(480, 480);
            this.panel1.TabIndex = 0;
            // 
            // ObjX
            // 
            this.ObjX.Location = new System.Drawing.Point(146, 37);
            this.ObjX.Name = "ObjX";
            this.ObjX.Size = new System.Drawing.Size(30, 20);
            this.ObjX.TabIndex = 1;
            this.ObjX.Leave += new System.EventHandler(this.Form1_Load);
            // 
            // ObjY
            // 
            this.ObjY.Location = new System.Drawing.Point(182, 37);
            this.ObjY.Name = "ObjY";
            this.ObjY.Size = new System.Drawing.Size(30, 20);
            this.ObjY.TabIndex = 2;
            this.ObjY.Leave += new System.EventHandler(this.Form1_Load);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Saisir X puis Y de l\'objet : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Saisir X puis Y de la destination : ";
            // 
            // DestY
            // 
            this.DestY.Location = new System.Drawing.Point(426, 37);
            this.DestY.Name = "DestY";
            this.DestY.Size = new System.Drawing.Size(30, 20);
            this.DestY.TabIndex = 5;
            this.DestY.Leave += new System.EventHandler(this.Form1_Load);
            // 
            // DestX
            // 
            this.DestX.Location = new System.Drawing.Point(390, 37);
            this.DestX.Name = "DestX";
            this.DestX.Size = new System.Drawing.Size(30, 20);
            this.DestX.TabIndex = 4;
            this.DestX.Leave += new System.EventHandler(this.Form1_Load);
            // 
            // SearchPath
            // 
            this.SearchPath.Location = new System.Drawing.Point(462, 36);
            this.SearchPath.Name = "SearchPath";
            this.SearchPath.Size = new System.Drawing.Size(50, 23);
            this.SearchPath.TabIndex = 7;
            this.SearchPath.Text = "Search";
            this.SearchPath.UseVisualStyleBackColor = true;
            this.SearchPath.Click += new System.EventHandler(this.SearchPath_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Taille du tableau X/Y";
            // 
            // tbTabY
            // 
            this.tbTabY.Enabled = false;
            this.tbTabY.Location = new System.Drawing.Point(182, 6);
            this.tbTabY.Name = "tbTabY";
            this.tbTabY.Size = new System.Drawing.Size(30, 20);
            this.tbTabY.TabIndex = 11;
            this.tbTabY.Leave += new System.EventHandler(this.Form1_Load);
            // 
            // tbTabX
            // 
            this.tbTabX.Enabled = false;
            this.tbTabX.Location = new System.Drawing.Point(146, 6);
            this.tbTabX.Name = "tbTabX";
            this.tbTabX.Size = new System.Drawing.Size(30, 20);
            this.tbTabX.TabIndex = 10;
            this.tbTabX.Leave += new System.EventHandler(this.Form1_Load);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(324, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Dessiner la map";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblInfoTime
            // 
            this.lblInfoTime.AutoSize = true;
            this.lblInfoTime.Location = new System.Drawing.Point(12, 611);
            this.lblInfoTime.Name = "lblInfoTime";
            this.lblInfoTime.Size = new System.Drawing.Size(79, 13);
            this.lblInfoTime.TabIndex = 14;
            this.lblInfoTime.Text = "Temps requis : ";
            // 
            // btnDEBUG
            // 
            this.btnDEBUG.Location = new System.Drawing.Point(345, 576);
            this.btnDEBUG.Name = "btnDEBUG";
            this.btnDEBUG.Size = new System.Drawing.Size(75, 23);
            this.btnDEBUG.TabIndex = 15;
            this.btnDEBUG.Text = "DEBUG";
            this.btnDEBUG.UseVisualStyleBackColor = true;
            this.btnDEBUG.Click += new System.EventHandler(this.btnDEBUG_Click);
            // 
            // lblDEBUG
            // 
            this.lblDEBUG.AutoSize = true;
            this.lblDEBUG.Location = new System.Drawing.Point(425, 581);
            this.lblDEBUG.Name = "lblDEBUG";
            this.lblDEBUG.Size = new System.Drawing.Size(87, 13);
            this.lblDEBUG.TabIndex = 16;
            this.lblDEBUG.Text = "Etat actuel : OFF";
            // 
            // tbxErrorLog
            // 
            this.tbxErrorLog.Location = new System.Drawing.Point(12, 627);
            this.tbxErrorLog.Name = "tbxErrorLog";
            this.tbxErrorLog.ReadOnly = true;
            this.tbxErrorLog.Size = new System.Drawing.Size(500, 20);
            this.tbxErrorLog.TabIndex = 17;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(276, 575);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(63, 24);
            this.button2.TabIndex = 18;
            this.button2.Text = "QUITTER";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.ForeColor = System.Drawing.Color.DarkViolet;
            this.lblY.Location = new System.Drawing.Point(27, 53);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(14, 13);
            this.lblY.TabIndex = 19;
            this.lblY.Text = "Y";
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.ForeColor = System.Drawing.Color.Orange;
            this.lblX.Location = new System.Drawing.Point(9, 82);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(14, 13);
            this.lblX.TabIndex = 20;
            this.lblX.Text = "X";
            // 
            // btnLoadmapChoice
            // 
            this.btnLoadmapChoice.Location = new System.Drawing.Point(224, 4);
            this.btnLoadmapChoice.Name = "btnLoadmapChoice";
            this.btnLoadmapChoice.Size = new System.Drawing.Size(94, 23);
            this.btnLoadmapChoice.TabIndex = 21;
            this.btnLoadmapChoice.Text = "Choisir la map";
            this.btnLoadmapChoice.UseVisualStyleBackColor = true;
            this.btnLoadmapChoice.Click += new System.EventHandler(this.InstanceChoixMap);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(146, 576);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(124, 23);
            this.button3.TabIndex = 22;
            this.button3.Text = "Afficher le Debug";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 66);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "--------->";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(14, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "V";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(9, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "|";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 108);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(9, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "|";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(30, 576);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(110, 23);
            this.button4.TabIndex = 25;
            this.button4.Text = "Remise à zéro";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(426, 4);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(95, 23);
            this.button5.TabIndex = 26;
            this.button5.Text = "Précision double";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // lblPrecis
            // 
            this.lblPrecis.AutoSize = true;
            this.lblPrecis.Location = new System.Drawing.Point(425, 594);
            this.lblPrecis.Name = "lblPrecis";
            this.lblPrecis.Size = new System.Drawing.Size(96, 13);
            this.lblPrecis.TabIndex = 27;
            this.lblPrecis.Text = "Précision : normale";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 662);
            this.ControlBox = false;
            this.Controls.Add(this.lblPrecis);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnLoadmapChoice);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.lblY);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tbxErrorLog);
            this.Controls.Add(this.lblDEBUG);
            this.Controls.Add(this.btnDEBUG);
            this.Controls.Add(this.lblInfoTime);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbTabY);
            this.Controls.Add(this.tbTabX);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SearchPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DestY);
            this.Controls.Add(this.DestX);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ObjY);
            this.Controls.Add(this.ObjX);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(540, 700);
            this.MinimumSize = new System.Drawing.Size(540, 700);
            this.Name = "Form1";
            this.Text = "Démo du PathFinder";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox ObjX;
        private System.Windows.Forms.TextBox ObjY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DestY;
        private System.Windows.Forms.TextBox DestX;
        private System.Windows.Forms.Button SearchPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTabY;
        private System.Windows.Forms.TextBox tbTabX;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblInfoTime;
        private System.Windows.Forms.Button btnDEBUG;
        private System.Windows.Forms.Label lblDEBUG;
        private System.Windows.Forms.TextBox tbxErrorLog;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Button btnLoadmapChoice;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label lblPrecis;
    }
}

