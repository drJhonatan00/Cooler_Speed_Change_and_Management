using LibreHardwareMonitor.Hardware;
using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        string ac = "C";
        private int seconds = 0;
        private const int maxGraphicPoints = 100;
        private Computer pc;
        private bool _oi = false;
        string cash = "25";
        string kaosu = "";
        int ptg = 0;
        string cam = Path.Combine(AppContext.BaseDirectory, "WindowsForms6.config");
        string tempet = Path.Combine(AppContext.BaseDirectory, "WindowsFormsApp6.pdb.config");

        public Form1()
        {
            InitializeComponent();
            ConfigUltimate();
            GraphicConfig();
            GraphicConfigRpm();
            LabelRpmActualize();
            HardWare();
            ControlConfig();
        }

        private void ConfigUltimate()
        {
            try
            {
                if (!File.Exists(cam)) File.WriteAllText(cam, "EN");
                if (!File.Exists(tempet)) File.WriteAllText(tempet, "C");
            }
            catch { }
        }

        private string SecureFile(string caminho, string padrao)
        {
            try { return File.Exists(caminho) ? File.ReadAllText(caminho).Trim() : padrao; }
            catch { return padrao; }
        }

        private void HardWare()
        {
            pc = new Computer
            {
                IsCpuEnabled = true,
                IsMotherboardEnabled = true,
                IsControllerEnabled = true,
                IsMemoryEnabled = true,
                IsGpuEnabled = true
            };
            pc.Open();
            System.Threading.Thread.Sleep(500);
            pc.Accept(new UpdateVisitor());
            GhostInTheShell();
        }

        private float UniversalTemp()
        {
            foreach (IHardware hardware in pc.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {

                    foreach (ISensor sensā in hardware.Sensors)
                    {
                        if (sensā.SensorType == SensorType.Temperature && sensā.Value.HasValue)
                        {
                            string nome = sensā.Name.ToLower();
                            if (nome.Contains("package") || nome.Contains("core average"))
                            {
                                return sensā.Value.Value;
                            }
                        }
                    }


                    foreach (ISensor sensā in hardware.Sensors)
                    {
                        if (sensā.SensorType == SensorType.Temperature && sensā.Value.HasValue)
                        {
                            if (!sensā.Name.ToLower().Contains("distance"))
                            {
                                return sensā.Value.Value;
                            }
                        }
                    }
                }
            }
            return 0f;
        }
        private void ActualizeViewerRpm(int jikan)
        {
            float rpm = 0;

            if (cmbTohou.SelectedItem is ItemControleFan debaisu)
            {
                if (debaisu.otherGoodSensor != null && debaisu.otherGoodSensor.Value.HasValue)
                {
                    rpm = debaisu.otherGoodSensor.Value.Value;
                }
            }

            if (chartCooler.Series.IndexOf("K") != -1)
            {
                Series serie = chartCooler.Series["K"];


                serie.Points.AddXY(jikan, rpm);

                if (serie.Points.Count > maxGraphicPoints)
                {
                    serie.Points.RemoveAt(0);

                    chartCooler.ChartAreas[0].AxisX.Minimum = jikan - maxGraphicPoints;
                    chartCooler.ChartAreas[0].AxisX.Maximum = jikan;
                }

                if (rpm > chartCooler.ChartAreas[0].AxisY.Maximum)
                {
                    chartCooler.ChartAreas[0].AxisY.Maximum = Math.Ceiling(rpm / 500.0) * 500;
                }
            }
        }


        private void ControlConfig()
        {
            fanSuraidaa.ValueChanged += SliderTemperatura_ValueChanged;
            rifuresshuRēto.Tick += TimerAtualizacao_Tick;
            rifuresshuRēto.Start();
        }

        private void GraphicConfig()
        {

            ondoGurafu.Series.Clear();
            ondoGurafu.ChartAreas.Clear();

            ChartArea area = new ChartArea("AreaTemp");
            area.AxisX.Title = "";
            area.AxisY.Title = "";

            if (ac == "C")
            {
                area.AxisY.Minimum = 0;
                area.AxisY.Maximum = 120;
            }
            else if (ac == "F")
            {
                area.AxisY.Minimum = 32;
                area.AxisY.Maximum = 248;
            }
            else if (ac == "K")
            {
                area.AxisY.Minimum = 273.15;
                area.AxisY.Maximum = 393.15;
            }

            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            ondoGurafu.ChartAreas.Add(area);

            Series serie = new Series("Temperatura")
            {
                
                ChartType = SeriesChartType.FastLine,
                BorderWidth = 3,
                Color = Color.Red,
                XValueType = ChartValueType.Int32
            };
            ondoGurafu.Series.Add(serie);
        }

        private void SliderTemperatura_ValueChanged(object sender, EventArgs e)
        {

        }

        private async void TimerAtualizacao_Tick(object sender, EventArgs e)
        {

            if (_oi) return;
            _oi = true;

            try
            {

                await Task.Run(() =>
                {
                    pc.Accept(new UpdateVisitor());
                });

                seconds++;


                ControlApplicationFanShadow(fanSuraidaa.Value);


                float unrealTime = UniversalTemp();
                double tempView = unrealTime;
                if (radioButton1.Checked)
                {
                    lblOndoVal.Text = $"{unrealTime:F1} °C";
                    tempView = unrealTime;
                }
                else if (radioButton2.Checked)
                {
                    tempView = (unrealTime * 1.8) + 32;
                    double tempF = (unrealTime * 1.8) + 32;
                    lblOndoVal.Text = $"{tempF:F1} °F";

                }
                else if (radioButton3.Checked)
                {
                    tempView = unrealTime + 273.15;
                    double tempK = unrealTime + 273.15;
                    lblOndoVal.Text = $"{tempK:F1} °K";
                }

                var serieTemp = ondoGurafu.Series["Temperatura"];
                serieTemp.Points.AddXY(seconds, tempView);

                if (serieTemp.Points.Count > maxGraphicPoints)
                {
                    serieTemp.Points.RemoveAt(0);
                    ondoGurafu.ChartAreas[0].AxisX.Minimum = seconds - maxGraphicPoints;
                    ondoGurafu.ChartAreas[0].AxisX.Maximum = seconds;
                }


                LabelRpmActualize();
                ActualizeViewerRpm(seconds);
            }
            catch
            {

            }
            finally
            {
                _oi = false;
            }
        }

        private void ControlApplicationFanShadow(int porcentagem)
        {

            if (cmbTohou.SelectedItem is ItemControleFan debaisu)
            {

                debaisu.aGoodSensor.Control?.SetSoftware(porcentagem);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string tem = SecureFile(tempet, "C");
            if (tem == "C")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
                ac = "C";
                GraphicConfig();
            }
            else if (tem == "F")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
                radioButton3.Checked = false;
                ac = "F";
                GraphicConfig();
            }
            else if (tem == "K")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = true;
                ac = "K";
                GraphicConfig();
            }
            string data = SecureFile(cam, "PT");
            if (data == "EN")
            {
                kaosu = "Power: ";
                dev.Text = "Developed by Dr. Jhonatan";
                label19.Text = "Language";
                label20.Text = "English";
                button1.Text = "Scanner";
                shokikaBtn.Text = "CPU Control";
                label15.Text = "Cooler Speed";
                label14.Text = "Processor Temperature";
                label16.Text = "Processor\nTemperature";
                label17.Text = "Cooler Speed";
                label12.Text = "Settings";
                button2.Text = "Change Language";
                label18.Text = kaosu + cash + "%";
                button3.Text = "Donate to developer";
            }
            else if (data == "ES")
            {
                kaosu = "Potencia: ";
                dev.Text = "Desarrollado por Dr. Jhonatan";
                label19.Text = "Idioma";
                label20.Text = "Español";
                button1.Text = "Informe";
                shokikaBtn.Text = "Devolver el Control";
                label15.Text = "Velocidad del Cooler";
                label14.Text = "Temperatura Actual";
                label16.Text = "Temperatura\nActual";
                label17.Text = "Velocidad del\nCooler";
                label12.Text = "Configuración";
                button2.Text = "Cambiar Idioma";
                label18.Text = kaosu + cash + "%";
                button3.Text = "Dona al desarrollador";
            }
            else if (data == "KO")
            {
                kaosu = "힘 ";
                dev.Text = "조나단 박사가 개발했습니다.";
                label19.Text = "언어";
                label20.Text = "한국어";
                button1.Text = "세부";
                shokikaBtn.Text = "CPU로 제어권을 반환합니다.";
                label15.Text = "냉각 속도";
                label14.Text = "현재 온도";
                label16.Text = "현재 온도";
                label17.Text = "냉각 속도";
                label12.Text = "설정";
                button2.Text = "언어 변경";
                label18.Text = kaosu + cash + "%";
                button3.Text = "개발자에게 기부하세요";
            }
            else if (data == "JA")
            {
                kaosu = "力 ";
                dev.Text = "ジョナサン博士によって開発されました";
                label19.Text = "言語";
                label20.Text = "日本語";
                button1.Text = "報告";
                shokikaBtn.Text = "CPU制御";
                label15.Text = "冷却速度";
                label14.Text = "プロセッサの温度";
                label16.Text = "プロセッサの温度";
                label17.Text = "冷却速度";
                label12.Text = "設定";
                button2.Text = "言語を変更する";
                label18.Text = kaosu + cash + "%";
                button3.Text = "開発者に寄付する";
            }
            else if (data == "CH")
            {
                kaosu = "力量 ";
                dev.Text = "由乔纳森博士开发";
                label19.Text = "语言";
                label20.Text = "简体中文";
                button1.Text = "报告";
                shokikaBtn.Text = "CPU控制";
                label15.Text = "冷却速度";
                label14.Text = "处理器温度";
                label16.Text = "处理器温度";
                label17.Text = "冷却速度";
                label12.Text = "设置";
                button2.Text = "更改语言";
                label18.Text = kaosu + cash + "%";
                button3.Text = "向开发者捐款";
            }
            else if (data == "DE")
            {
                kaosu = "Leistung: ";
                dev.Text = "Entwickelt von Dr. Jhonatan";
                label19.Text = "Sprache";
                label20.Text = "Deutsch";
                button1.Text = "Bericht";
                shokikaBtn.Text = "CPU-Steuerung";
                label15.Text = "Kühlergeschwindigkeit";
                label14.Text = "Aktuelle Temperatur";
                label16.Text = "Aktuelle\nTemperatur";
                label17.Text = "Kühle\nGeschwindigkeit";
                label12.Text = "Einstellungen";
                button2.Text = "Sprache ändern";
                label18.Text = kaosu + cash + "%";
                button3.Text = "Spenden Sie an den Entwickler";
            }
            else if (data == "ITA")
            {
                kaosu = "Potenza: ";
                dev.Text = "Sviluppato dal Dott. Jhonatan";
                label19.Text = "Lingua";
                label20.Text = "Italiano";
                button1.Text = "Rapporto";
                shokikaBtn.Text = "Controllo CPU";
                label15.Text = "Velocità più Fredda";
                label14.Text = "Temperatura Attuale";
                label16.Text = "Temperatura\nAttuale";
                label17.Text = "Velocità più\nFredda";
                label12.Text = "Impostazioni";
                button2.Text = "Cambia Lingua";
                label18.Text = kaosu + cash + "%";
                button3.Text = "Fai una donazione allo sviluppatore";
            }
            else if (data == "FR")
            {
                kaosu = "Pouvoir: ";
                dev.Text = "Développé par le Dr Jhonatan";
                label19.Text = "Langue";
                label20.Text = "Français";
                button1.Text = "Rapport";
                shokikaBtn.Text = "Contrôle du Processeur";
                label15.Text = "Vitesse du refroidisseur";
                label14.Text = "Température actuelle";
                label16.Text = "Température\nActualle";
                label17.Text = "Vitesse du\nrefroidisseur";
                label12.Text = "Paramètres";
                button2.Text = "Changer de Langue";
                label18.Text = kaosu + cash + "%";
                button3.Text = "Faites un don au développeur";
            }
            else
            {
                kaosu = "Potência: ";
                dev.Text = "Desenvolvido por Dr. Jhonatan";
                label19.Text = "Idioma";
                label20.Text = "Português";
                button1.Text = "Relatório";
                shokikaBtn.Text = "Restaurar";
                label15.Text = "Velocidade Cooler";
                label14.Text = "Temperatura atual";
                label16.Text = "Temperatura\nAtual";
                label17.Text = "Velocidade\nCooler";
                label12.Text = "Configurações";
                button2.Text = "Mudar Idioma";
                label18.Text = kaosu + cash + "%";
                button3.Text = "Doar para desenvolvedor";
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (pc == null) { base.OnFormClosed(e); return; }
            foreach (IHardware hardware in pc.Hardware)
            {
                foreach (ISensor sensā in hardware.Sensors)
                {
                    if (sensā.SensorType == SensorType.Control)
                        sensā.Control?.SetDefault();
                }
            }

            pc?.Close();
            base.OnFormClosed(e);
        }
        private void GraphicConfigRpm()
        {
            chartCooler.Series.Clear();

            Series serieCooler = chartCooler.Series.Add("K");
            serieCooler.ChartType = SeriesChartType.FastLine;
            serieCooler.Color = Color.BlueViolet;
            serieCooler.BorderWidth = 2;


            ChartArea area = chartCooler.ChartAreas[0];
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 2500;
        }
        private void sliderTemperatura_Scroll(object sender, EventArgs e)
        {
            string data = SecureFile(cam, "PT");
            if (fanSuraidaa.Value == 20)
            {

                if (data == "EN")
                {
                    MessageBox.Show("Some modern CPUs prevent the cooling fan speed from being reduced below 20% of its power. However, we do not recommend lowering it, and we accept no responsibility for any issues that may arise from reducing the power to less than 20%.", "Stern Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "ES")
                {
                    MessageBox.Show("Algunos procesadores modernos impiden que la velocidad del disipador se reduzca por debajo del 20% de su kaosu. Sin embargo, no recomendamos reducirla aún más y no nos hacemos responsables de los problemas que pueda surgir al reducir la kaosu por debajo del 20%.", "Severa Advetencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "KO")
                {
                    MessageBox.Show("일부 최신 CPU는 쿨러 속도를 전력 소모량의 20% 미만으로 낮추는 것을 방지합니다. 하지만 저희는 속도를 20% 미만으로 낮추는 것을 권장하지 않으며, 20% 미만으로 낮춤으로써 발생할 수 있는 문제에 대해서는 책임지지 않습니다.", "엄중한 경고", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "JA")
                {
                    MessageBox.Show("最新のCPUの中には、冷却速度を定格電力の20%未満に下げられないようにするものがあります。しかしながら、それ以上の速度低下は推奨しません。また、定格電力を20%未満に下げた際に発生するいかなる問題についても、当社は一切責任を負いません。", "厳しい警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "DE")
                {
                    MessageBox.Show("Einige moderne CPUs verhindern, dass die Lüfterdrehzahl unter 20 % ihrer Nennleistung reduziert wird. Wir raten jedoch davon ab, die Drehzahl weiter zu senken, und übernehmen keine Haftung für etwaige Probleme, die durch eine Reduzierung unter 20 % entstehen.", "Strenge Warnung", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "ITA")
                {
                    MessageBox.Show("Alcune CPU moderne impediscono di ridurre la velocità del dissipatore al di sotto del 20% della sua potenza. Tuttavia, sconsigliamo di ridurla ulteriormente e non ci assumiamo alcuna responsabilità per eventuali problemi che potrebbero verificarsi riducendo la potenza al di sotto del 20%.", "Severo Avvertimento", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "FR")
                {
                    MessageBox.Show("Certains processeurs modernes empêchent la réduction de la vitesse du système de refroidissement en dessous de 20 % de sa puissance. Toutefois, nous vous déconseillons de la réduire davantage et nous déclinons toute responsabilité en cas de problèmes rencontrés suite à une réduction de la puissance inférieure à 20%.", "Avertissement Sévère", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "CH")
                {
                    MessageBox.Show("某些现代CPU会限制散热器的运行速度，使其无法低于20%的功率。但是，我们不建议您进一步降低散热器的运行速度，并且对于因将功率降低到20%以下而可能出现的任何问题，我们概不负责。", "严厉警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "PT")
                {
                    MessageBox.Show("Algumas CPUs modernas impedem a redução da velocidade do cooler a menos que 20% de sua potência. Portanto não se recomenda que haja diminuição, não há responsabilidade de nossa parte por possíveis problemas que possas obter ao reduzir a potência a um número menor que 20%", "Aviso Severo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            ptg = fanSuraidaa.Value;
            cash = ptg.ToString();
            label18.Text = kaosu + cash + "%";

        }

        private float AddRpmShadow()
        {
            foreach (IHardware hw in pc.Hardware)
            {

                foreach (ISensor s in hw.Sensors)
                {
                    if (s.SensorType == SensorType.Fan && s.Value.HasValue && s.Value.Value > 0)
                    {
                        return s.Value.Value;
                    }
                }


                foreach (IHardware subHw in hw.SubHardware)
                {
                    foreach (ISensor s in subHw.Sensors)
                    {
                        if (s.SensorType == SensorType.Fan && s.Value.HasValue && s.Value.Value > 0)
                        {
                            return s.Value.Value;
                        }
                    }
                }
            }
            return 0f;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = SecureFile(cam, "PT");
            var shindan = new System.Text.StringBuilder();

            if (data == "EN")
            {
                shindan.AppendLine("=== COMPLETE SENSOR REPORT ===\n");
            }
            else if (data == "ES")
            {
                shindan.AppendLine("=== INFORME COMPLETO DEL SENSOR ===\n");
            }
            else if (data == "KO")
            {
                shindan.AppendLine("=== 센서 종합 보고서 ===\n");
            }
            else if (data == "JA")
            {
                shindan.AppendLine("=== センサーレポート全文 ===\n");
            }
            else if (data == "DE")
            {
                shindan.AppendLine("=== VOLLSTÄNDIGER SENSORBERICHT ===\n");
            }
            else if (data == "ITA")
            {
                shindan.AppendLine("=== REPORT COMPLETO DEL SENSORE ===\n");
            }
            else if (data == "FR")
            {
                shindan.AppendLine("=== RAPPORT COMPLET SUR LES CAPTEURS ===\n");
            }
            else if (data == "CH")
            {
                shindan.AppendLine("=== 完整传感器报告 ===\n");
            }
            else
            {
                shindan.AppendLine("=== RELATÓRIO COMPLETO DE SENSORES ===\n");
            }


            foreach (IHardware hw in pc.Hardware)
            {
                hw.Update();
                shindan.AppendLine($"[ {hw.Name} ] ({hw.HardwareType})");


                var sensāGurūpu = hw.Sensors.GroupBy(s => s.SensorType);

                foreach (var gurūpu in sensāGurūpu)
                {
                    shindan.AppendLine($"  -- Type: {gurūpu.Key} --");
                    foreach (ISensor sensā in gurūpu)
                    {
                        if (sensā.Value.HasValue)
                        {
                            shindan.AppendLine($"     • {sensā.Name}: {sensā.Value.Value:F1}");
                        }
                    }
                }


                foreach (IHardware subHw in hw.SubHardware)
                {
                    subHw.Update();
                    shindan.AppendLine($"\n  [ Sub-Hardware: {subHw.Name} ]");

                    foreach (ISensor subSensor in subHw.Sensors)
                    {
                        if (subSensor.Value.HasValue)
                        {
                            shindan.AppendLine($"     • {subSensor.SensorType} | {subSensor.Name}: {subSensor.Value.Value:F1}");
                        }
                    }
                }

                shindan.AppendLine(new string('-', 40));
            }


            ExibirJanelaLog(shindan.ToString());
        }
        private void ExibirJanelaLog(string textoLog)
        {
            Form formLog = new Form
            {
                Text = "System Diagnostic",
                Size = new Size(600, 500),
                StartPosition = FormStartPosition.CenterParent
            };

            TextBox txtLog = new TextBox
            {
                Multiline = true,
                ReadOnly = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                Text = textoLog,
                Font = new Font("Consolas", 10)
            };

            formLog.Controls.Add(txtLog);
            formLog.ShowDialog();
        }
        private void LabelRpmActualize()
        {
            if (cmbTohou.SelectedItem is ItemControleFan debaisu)
            {

                if (debaisu.otherGoodSensor != null && debaisu.otherGoodSensor.Value.HasValue)
                {
                    lblRpm.Text = $"{debaisu.otherGoodSensor.Name}: {debaisu.otherGoodSensor.Value.Value:F0} RPM";
                    return;
                }
            }

            lblRpm.Text = "Cooler: 0 RPM";
        }
        private void GhostInTheShell()
        {
            cmbTohou.Items.Clear();

            foreach (IHardware hardware in pc.Hardware)
            {

                var controls = hardware.Sensors.Where(s => s.SensorType == SensorType.Control).ToList();
                var kaitensuu = hardware.Sensors.Where(s => s.SensorType == SensorType.Fan).ToList();

                for (int i = 0; i < controls.Count; i++)
                {

                    ISensor taiōzuke = (i < kaitensuu.Count) ? kaitensuu[i] : null;

                    cmbTohou.Items.Add(new ItemControleFan
                    {
                        hyōjimei = $"{hardware.Name} - {controls[i].Name}",
                        heirHardware = hardware,
                        aGoodSensor = controls[i],
                        otherGoodSensor = taiōzuke
                    });
                }
            }

            if (cmbTohou.Items.Count > 0)
            {
                cmbTohou.SelectedIndex = 0;
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void btnResetar_Click(object sender, EventArgs e)
        {
            string data = SecureFile(cam, "PT");
            foreach (var hw in pc.Hardware)
            {
                foreach (var s in hw.Sensors.Where(x => x.SensorType == SensorType.Control))
                {
                    s.Control?.SetDefault();
                }
            }
            fanSuraidaa.Value = 25;
            if (data == "EN")
            {
                MessageBox.Show("Control returned to BIOS", "Warning");

            }
            else if (data == "ES")
            {
                MessageBox.Show("El control volvió a la BIOS", "Aviso");

            }
            else if (data == "KO")
            {
                MessageBox.Show("제어권이 BIOS로 돌아왔습니다.", "알아채다");

            }
            else if (data == "JA")
            {
                MessageBox.Show("制御はBIOSに戻った", "知らせ");

            }
            else if (data == "DE")
            {
                MessageBox.Show("Die Kontrolle wurde an das BIOS zurückgegeben.", "Beachten");

            }
            else if (data == "ITA")
            {
                MessageBox.Show("Controllo ripristinato al BIOS", "Avviso");

            }
            else if (data == "FR")
            {
                MessageBox.Show("Le contrôle a été rendu au BIOS.", "Avis");

            }
            else if (data == "CH")
            {
                MessageBox.Show("控制权已返回BIOS", "注意");

            }
            else if (data == "PT")
            {
                MessageBox.Show("Controle devolvido para BIOS", "Aviso");

            }
        }

        private void label19_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form = new Form2();
            form.ShowDialog();
            this.Close();
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form = new Form2();
            form.ShowDialog();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string data = SecureFile(cam, "PT");
            if (lblRpm.Text == "Cooler: 0 RPM")
            {
                if (data == "EN")
                {
                    MessageBox.Show("This application must be run as an administrator; otherwise, it will not be able to communicate with the motherboard.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else if (data == "ES")
                {
                    MessageBox.Show("Esta aplicación debe abrirse como administrador; de lo contrario, no podrá comunicarse con la placa base.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else if (data == "KO")
                {
                    MessageBox.Show("이 애플리케이션은 관리자 권한으로 실행해야 합니다. 그렇지 않으면 마더보드와 통신할 수 없습니다.", "알아채다", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else if (data == "JA")
                {
                    MessageBox.Show("このアプリケーションは管理者権限で起動する必要があります。そうしないと、マザーボードと通信できません。", "知らせ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else if (data == "DE")
                {
                    MessageBox.Show("Diese Anwendung muss als Administrator geöffnet werden; andernfalls kann sie nicht mit dem Motherboard kommunizieren.", "Beachten", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else if (data == "ITA")
                {
                    MessageBox.Show("Questa applicazione deve essere aperta come amministratore; in caso contrario, non sarà in grado di comunicare con la scheda madre.", "Avviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else if (data == "FR")
                {
                    MessageBox.Show("Cette application doit être ouverte en tant qu'administrateur ; sinon, elle ne pourra pas communiquer avec la carte mère.", "Avis", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else if (data == "CH")
                {
                    MessageBox.Show("此应用程序必须以管理员身份运行；否则，它将无法与主板通信。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else if (data == "PT")
                {
                    MessageBox.Show("Esse aplicativo deve ser aberto como Administrador, caso contrário, não poderá se comunicar com a placa-mãe", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    timer1.Stop();
                }
                else { timer1.Stop(); }

            }
            else
            {
                timer1.Stop();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            File.WriteAllText(tempet, "F");
            float unrealTime = UniversalTemp();
            double tempF = (unrealTime * 1.8) + 32;
            lblOndoVal.Text = $"{tempF:F1} °F";
            ac = "F";
            GraphicConfig();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            File.WriteAllText(tempet, "C");
            ac = "C";
            GraphicConfig();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            File.WriteAllText(tempet, "K");
            ac = "K";
            GraphicConfig();
        }
    }
    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer) { computer.Traverse(this); }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware sub in hardware.SubHardware) sub.Accept(this);
        }
        public void VisitSensor(ISensor sensā) { }
        public void VisitParameter(IParameter parameter) { }
    }
    public class ItemControleFan
    {
        public string hyōjimei { get; set; }
        public IHardware heirHardware { get; set; }
        public ISensor aGoodSensor { get; set; }
        public ISensor otherGoodSensor { get; set; }

        public override string ToString()
        {
            return hyōjimei;
        }
    }
}


//Watashi wa watashi sore dake

//Mitekudasai watashi ga koko ni komento o nokoshite iru no wa
//minasan ga hontōni watashi no shusshin-chi o shirita gatte irukaradesu.
//Sore ni dōse dare mo komento o yomanainode kiken wa nai to omotte imasu.
//Watashi wa shōshinshōmei no nihonkokumin de Nihon de nihonjin no
//ryōshin no moto ni umaremashita.
//Kenburijjidaigaku de benkyō shite ita 12-nenkan Igirisu ni sunde imashita.
//Soshite furusato no Nihon ni modotte kimashita.
//Nihonjin'nara sudeni shitte iru ikutsu ka no riyū kara seikaku
//ni wa shusshin-chi o iimasen. Demo, watashi ga amerikahitoda to
//ii tsudzukete iru komyuniti ni meikaku ni shite okitai no wa tatoe
//watashi ga kōdo o gisō shite ite mo, watashi wa izentoshite
//nihonjinda to iu kotodesu. Sore ga rikai dekirunara… monodzuruki.