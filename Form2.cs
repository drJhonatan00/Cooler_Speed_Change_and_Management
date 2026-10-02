using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp6
{
    public partial class Form2 : Form
    {
        string barry = @"WindowsForms6.config";
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "EN");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "ES");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "PT");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "KO");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "JA");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "DE");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "ITA");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "FR");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            File.WriteAllText(barry, "CH");
            this.Hide();
            Form1 form = new Form1();
            form.ShowDialog();
            this.Close();
        }

        private void tsuno_o_magaru(object sender, PaintEventArgs e)
        {
            Panel pn1 = (Panel)sender;
            int kyokuritsu = 5;
            int d = kyokuritsu * 2;

            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (System.Drawing.Drawing2D.GraphicsPath shiawase = new System.Drawing.Drawing2D.GraphicsPath())
            {
                shiawase.AddArc(0, 0, d, d, 180, 90);
                shiawase.AddArc(pn1.Width - d - 1, 0, d, d, 270, 90);
                shiawase.AddArc(pn1.Width - d - 1, pn1.Height - d - 1, d, d, 0, 90);
                shiawase.AddArc(0, pn1.Height - d - 1, d, d, 90, 90);
                shiawase.CloseAllFigures();

                pn1.Region = new Region(shiawase);
            }
        }
    }
}


//Watashi wa watashi sore dake