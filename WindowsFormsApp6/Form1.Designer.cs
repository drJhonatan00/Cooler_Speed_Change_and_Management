using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Security.Policy;
using System.Windows.Forms;
using WindowsFormsApp6.Properties;

namespace WindowsFormsApp6
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager res = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.Icon = ((System.Drawing.Icon)(res.GetObject("$this.Icon")));
            this.components = new System.ComponentModel.Container();
            var areaTemp = new System.Windows.Forms.DataVisualization.Charting.ChartArea("AreaTemp");
            var areaFan = new System.Windows.Forms.DataVisualization.Charting.ChartArea("AreaFan");
            var seriesTemp = new System.Windows.Forms.DataVisualization.Charting.Series("Temperatura");
            var seriesFan = new System.Windows.Forms.DataVisualization.Charting.Series("Cooler");
            this.lblOndoVal = new System.Windows.Forms.Label();
            this.ondoGurafu = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.rifuresshuRēto = new System.Windows.Forms.Timer(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.lblRpm = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cmbTohou = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.dev = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel5 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.shokikaBtn = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.xampp = new System.Windows.Forms.Label();
            this.chartCooler = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.linklabel1 = new System.Windows.Forms.LinkLabel();
            this.fanSuraidaa = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label(); this.label3 = new System.Windows.Forms.Label(); this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label(); this.label6 = new System.Windows.Forms.Label(); this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label(); this.label9 = new System.Windows.Forms.Label(); this.label10 = new System.Windows.Forms.Label(); this.label11 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.ondoGurafu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartCooler)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.fanSuraidaa)).BeginInit();
            this.SuspendLayout();

            var bg = System.Drawing.Color.FromArgb(15, 23, 42);
            var surface = System.Drawing.Color.FromArgb(30, 41, 59);
            var surface2 = System.Drawing.Color.FromArgb(36, 50, 73);
            var accent = System.Drawing.Color.FromArgb(56, 189, 248);
            var text = System.Drawing.Color.FromArgb(226, 232, 240);
            var muted = System.Drawing.Color.FromArgb(148, 163, 184);
            var font = new System.Drawing.Font("Segoe UI", 10F);
            var title = new System.Drawing.Font("Segoe UI Semibold", 11F);

            


            this.BackColor = bg; this.ClientSize = new System.Drawing.Size(1280, 760); this.MinimumSize = new System.Drawing.Size(1080, 680);
            this.Font = font; this.ForeColor = text; this.Name = "Form1"; this.Text = "Cooler Speed Change and Management"; this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable; this.Load += new System.EventHandler(this.Form1_Load);

            this.panel3.BackColor = System.Drawing.Color.FromArgb(17, 29, 49); this.panel3.Dock = System.Windows.Forms.DockStyle.Left; this.panel3.Width = 285; this.panel3.Padding = new System.Windows.Forms.Padding(22, 24, 22, 18); this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            this.label12.Text = "知らせ"; this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 13F); this.label12.ForeColor = accent; this.label12.AutoSize = true; this.label12.Location = new System.Drawing.Point(22, 25);
            this.xampp.Text = "Cooler Speed Change Management"; this.xampp.Font = title; this.xampp.ForeColor = text; this.xampp.AutoSize = true; this.xampp.Location = new System.Drawing.Point(22, 85);
            this.cmbTohou.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList; this.cmbTohou.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.cmbTohou.BackColor = surface2; this.cmbTohou.ForeColor = text; this.cmbTohou.Location = new System.Drawing.Point(22, 116); this.cmbTohou.Size = new System.Drawing.Size(241, 31);
            this.label18.Text = "知らせ"; this.label18.Font = title; this.label18.ForeColor = accent; this.label18.AutoSize = true; this.label18.Location = new System.Drawing.Point(22, 174);
            this.shokikaBtn.Text = "知らせ"; this.shokikaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.shokikaBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(71, 85, 105); this.shokikaBtn.ForeColor = text; this.shokikaBtn.BackColor = surface2; this.shokikaBtn.Location = new System.Drawing.Point(22, 208); this.shokikaBtn.Size = new System.Drawing.Size(241, 42); this.shokikaBtn.Click += new System.EventHandler(this.btnResetar_Click);
            this.button1.Text = "知らせ"; this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.button1.FlatAppearance.BorderColor = accent; this.button1.ForeColor = accent; this.button1.BackColor = System.Drawing.Color.Transparent; this.button1.Location = new System.Drawing.Point(22, 262); this.button1.Size = new System.Drawing.Size(241, 42); this.button1.Click += new System.EventHandler(this.button1_Click);
            this.radioButton1.Text = "°C"; this.radioButton2.Text = "°F"; this.radioButton3.Text = "K";
            foreach (var unit in new[] { this.radioButton1, this.radioButton2, this.radioButton3 }) { unit.AutoSize = true; unit.ForeColor = text; unit.BackColor = System.Drawing.Color.Black; unit.FlatStyle = System.Windows.Forms.FlatStyle.Flat; }
            this.radioButton1.Location = new System.Drawing.Point(22, 315); this.radioButton2.Location = new System.Drawing.Point(90, 315); this.radioButton3.Location = new System.Drawing.Point(158, 315); this.radioButton1.Checked = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged); this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged); this.radioButton3.CheckedChanged += new System.EventHandler(this.radioButton3_CheckedChanged);
            this.panel5.BackColor = surface; this.panel5.Location = new System.Drawing.Point(22, 335); this.panel5.Size = new System.Drawing.Size(241, 92); this.panel5.Padding = new System.Windows.Forms.Padding(12); this.panel5.Controls.Add(this.label19); this.panel5.Controls.Add(this.label20); this.panel5.Controls.Add(this.button2);
            this.label19.Text = "知らせ"; this.label19.Font = title; this.label19.ForeColor = text; this.label19.AutoSize = true; this.label19.Location = new System.Drawing.Point(12, 10);
            this.label20.Text = "知らせ"; this.label20.ForeColor = muted; this.label20.AutoSize = true; this.label20.Location = new System.Drawing.Point(12, 38);
            this.button2.Text = "知らせ"; this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.button2.FlatAppearance.BorderSize = 0; this.button2.ForeColor = accent; this.button2.BackColor = System.Drawing.Color.Transparent; this.button2.Location = new System.Drawing.Point(145, 26); this.button2.Size = new System.Drawing.Size(82, 32); this.button2.Click += new System.EventHandler(this.button2_Click);
            this.panel6.BackColor = System.Drawing.Color.FromArgb(22, 36, 59); this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom; this.panel6.Height = 68; this.panel6.Controls.Add(this.dev); this.panel6.Controls.Add(this.button3);
            this.dev.Text = "知らせ"; this.dev.Font = new System.Drawing.Font("Segoe UI", 8.5F); this.dev.ForeColor = muted; this.dev.AutoSize = true; this.dev.Location = new System.Drawing.Point(2, 11);
            this.button3.Text = "知らせ"; this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.button3.FlatAppearance.BorderSize = 0; this.button3.ForeColor = accent; this.button3.BackColor = System.Drawing.Color.Transparent; this.button3.Location = new System.Drawing.Point(0, 32); this.button3.Size = new System.Drawing.Size(130, 28); this.button3.Click += new System.EventHandler(this.button3_Click);
            this.panel3.Controls.Add(this.label12); this.panel3.Controls.Add(this.xampp); this.panel3.Controls.Add(this.cmbTohou); this.panel3.Controls.Add(this.label18); this.panel3.Controls.Add(this.shokikaBtn); this.panel3.Controls.Add(this.button1); this.panel3.Controls.Add(this.radioButton1); this.panel3.Controls.Add(this.radioButton2); this.panel3.Controls.Add(this.radioButton3); this.panel3.Controls.Add(this.panel5); this.panel3.Controls.Add(this.panel6);

            this.panel1.BackColor = surface; this.panel1.Location = new System.Drawing.Point(320, 28); this.panel1.Size = new System.Drawing.Size(300, 128); this.panel1.Padding = new System.Windows.Forms.Padding(20); this.panel1.Controls.Add(this.label14); this.panel1.Controls.Add(this.lblOndoVal);
            this.label14.Text = "知らせ"; this.label14.Font = title; this.label14.ForeColor = muted; this.label14.AutoSize = true; this.label14.Location = new System.Drawing.Point(20, 18);
            this.lblOndoVal.Text = "--.- °C"; this.lblOndoVal.Font = new System.Drawing.Font("Segoe UI Semibold", 28F); this.lblOndoVal.ForeColor = System.Drawing.Color.FromArgb(251, 146, 60); this.lblOndoVal.AutoSize = true; this.lblOndoVal.Location = new System.Drawing.Point(17, 47);
            this.panel2.BackColor = surface; this.panel2.Location = new System.Drawing.Point(640, 28); this.panel2.Size = new System.Drawing.Size(400, 128); this.panel2.Padding = new System.Windows.Forms.Padding(20); this.panel2.Controls.Add(this.label15); this.panel2.Controls.Add(this.lblRpm);
            this.label15.Text = "知らせ"; this.label15.Font = title; this.label15.ForeColor = muted; this.label15.AutoSize = true; this.label15.Location = new System.Drawing.Point(20, 18);
            this.lblRpm.Text = "0 RPM"; this.lblRpm.Font = new System.Drawing.Font("Segoe UI Semibold", 28F); this.lblRpm.ForeColor = accent; this.lblRpm.AutoSize = true; this.lblRpm.Location = new System.Drawing.Point(17, 47);

                        

            this.panel4.BackColor = System.Drawing.Color.Transparent; this.panel4.Location = new System.Drawing.Point(320, 180); this.panel4.Size = new System.Drawing.Size(920, 510);
            this.ondoGurafu.BackColor = System.Drawing.Color.WhiteSmoke; this.ondoGurafu.Location = new System.Drawing.Point(0, 0); this.ondoGurafu.Size = new System.Drawing.Size(445, 430); this.ondoGurafu.ChartAreas.Add(areaTemp); this.ondoGurafu.Series.Add(seriesTemp); this.ondoGurafu.Legends.Clear(); this.ondoGurafu.BorderlineColor = surface; this.ondoGurafu.BorderlineWidth = 0;
            this.chartCooler.BackColor = surface; this.chartCooler.Location = new System.Drawing.Point(475, 0); this.chartCooler.Size = new System.Drawing.Size(445, 430); this.chartCooler.ChartAreas.Add(areaFan); this.chartCooler.Series.Add(seriesFan); this.chartCooler.Legends.Clear(); this.chartCooler.BorderlineColor = surface; this.chartCooler.BorderlineWidth = 0;
            foreach (var chart in new[] { this.ondoGurafu, this.chartCooler }) { chart.ChartAreas[0].BackColor = surface; chart.ChartAreas[0].AxisX.LabelStyle.ForeColor = muted; chart.ChartAreas[0].AxisY.LabelStyle.ForeColor = muted; chart.ChartAreas[0].AxisX.LineColor = System.Drawing.Color.FromArgb(71, 85, 105); chart.ChartAreas[0].AxisY.LineColor = System.Drawing.Color.FromArgb(71, 85, 105); chart.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.FromArgb(51, 65, 85); chart.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.FromArgb(51, 65, 85); }
            seriesTemp.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine; seriesTemp.Color = System.Drawing.Color.FromArgb(251, 146, 60); seriesTemp.BorderWidth = 3; seriesTemp.ChartArea = "AreaTemp";
            seriesFan.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastLine; seriesFan.Color = accent; seriesFan.BorderWidth = 3; seriesFan.ChartArea = "AreaFan";
            this.label16.Text = "知らせ\n知らせ"; this.label16.Font = title; this.label16.ForeColor = muted; this.label16.AutoSize = true; this.label16.Location = new System.Drawing.Point(130, 445); this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label17.Text = "知らせ\n知らせ"; this.label17.Font = title; this.label17.ForeColor = muted; this.label17.AutoSize = true; this.label17.Location = new System.Drawing.Point(625, 445); this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.panel4.Controls.Add(this.ondoGurafu); this.panel4.Controls.Add(this.chartCooler); this.panel4.Controls.Add(this.label16); this.panel4.Controls.Add(this.label17);

            

            this.fanSuraidaa.Minimum = 0; this.fanSuraidaa.Maximum = 100; this.fanSuraidaa.Value = 25; this.fanSuraidaa.TickFrequency = 10; this.fanSuraidaa.Orientation = System.Windows.Forms.Orientation.Vertical; this.fanSuraidaa.Location = new System.Drawing.Point(1200, 28); this.fanSuraidaa.Size = new System.Drawing.Size(45, 128); this.fanSuraidaa.Scroll += new System.EventHandler(this.sliderTemperatura_Scroll); this.fanSuraidaa.ValueChanged += new System.EventHandler(this.SliderTemperatura_ValueChanged);
            this.label1.Text = "\n\n\n   Power"; this.label1.ForeColor = muted; this.label1.AutoSize = true; this.label1.Location = new System.Drawing.Point(1130, 28);
            
            var marks = new[] { this.label2, this.label3, this.label4, this.label5, this.label6, this.label7 }; for (int i = 0; i < marks.Length; i++) { marks[i].ForeColor = muted; marks[i].AutoSize = true; marks[i].Location = new System.Drawing.Point(1000, 52 + i * 21); }
            this.label8.Visible = false; this.label9.Visible = false; this.label10.Visible = false; this.label11.Visible = false;
            this.rifuresshuRēto.Interval = 10; this.rifuresshuRēto.Tick += new System.EventHandler(this.TimerAtualizacao_Tick); this.timer1.Interval = 10000; this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            this.Controls.Add(this.panel3); this.Controls.Add(this.panel1); this.Controls.Add(this.panel2); this.Controls.Add(this.panel4); this.Controls.Add(this.fanSuraidaa); this.Controls.Add(this.label1); this.Controls.Add(this.label2); this.Controls.Add(this.label3); this.Controls.Add(this.label4); this.Controls.Add(this.label5); this.Controls.Add(this.label6); this.Controls.Add(this.label7);
            this.linklabel1.Text = "drJhonatan00"; this.linklabel1.Location = new System.Drawing.Point(5, 5); this.linklabel1.AutoSize = true; this.linklabel1.ForeColor = muted; this.linklabel1.Visible = true; this.linklabel1.LinkClicked += (s, e) => Process.Start(new ProcessStartInfo("https://gitub.com/drJhonatan00"));
            ((System.ComponentModel.ISupportInitialize)(this.ondoGurafu)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.chartCooler)).EndInit(); ((System.ComponentModel.ISupportInitialize)(this.fanSuraidaa)).EndInit(); this.ResumeLayout(false); this.PerformLayout();
        }
        private Color TextSecondary => Color.FromArgb(164, 174, 192);
        private System.Windows.Forms.Label lblOndoVal, label1, label14, label15, lblRpm, dev, label20, label19, label18, label12, xampp, label16, label17, label2, label3, label4, label5, label6, label7, label8, label9, label10, label11;
        private System.Windows.Forms.DataVisualization.Charting.Chart ondoGurafu, chartCooler;
        private System.Windows.Forms.Panel panel1, panel2, panel3, panel4, panel5, panel6;
        private System.Windows.Forms.Timer rifuresshuRēto, timer1;
        private System.Windows.Forms.Button button1, button2, button3, shokikaBtn;
        private System.Windows.Forms.ComboBox cmbTohou;
        private System.Windows.Forms.TrackBar fanSuraidaa;
        private System.Windows.Forms.RadioButton radioButton1, radioButton2, radioButton3;
        private System.Windows.Forms.LinkLabel linklabel1;
     
    }
}

//Watashi wa watashi sore dake
