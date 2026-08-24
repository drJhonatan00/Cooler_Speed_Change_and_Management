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
        private int tempoDecorridoSegundos = 0;
        private const int MaxPontosNoGrafico = 30;
        private Computer pc;
        private bool _estaAtualizando = false;
        string cash = "25";
        string potencia = "";
        int ptg = 0;
        string cam = @"WindowsForms6.config";
        string tempet = @"WindowsFormsApp6.pdb.config";
        
        public Form1()
        {
            InitializeComponent();
            ConfigurarGrafico();
            ConfigurarGraficoRpm();
            AtualizarLabelRpm();
            InicializarHardware();
            ConfigurarControles();
        }

        private void InicializarHardware()
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
            CarregarDispositivosFan();
        }

        private float ObterTemperaturaUniversal()
        {
            foreach (IHardware hardware in pc.Hardware)
            {
                if (hardware.HardwareType == HardwareType.Cpu)
                {
                    
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Value.HasValue)
                        {
                            string nome = sensor.Name.ToLower();
                            if (nome.Contains("package") || nome.Contains("core average"))
                            {
                                return sensor.Value.Value;
                            }
                        }
                    }

                    
                    foreach (ISensor sensor in hardware.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature && sensor.Value.HasValue)
                        {
                            if (!sensor.Name.ToLower().Contains("distance"))
                            {
                                return sensor.Value.Value;
                            }
                        }
                    }
                }
            }
            return 0f;
        }
        private void AtualizarExibicaoRpm(int tempoAtual)
        {
            float rpmAtual = 0;

            if (cmbDispositivos.SelectedItem is ItemControleFan dispositivoSelecionado)
            {
                if (dispositivoSelecionado.SensorRpm != null && dispositivoSelecionado.SensorRpm.Value.HasValue)
                {
                    rpmAtual = dispositivoSelecionado.SensorRpm.Value.Value;
                }
            }

            if (chartCooler.Series.IndexOf("LinhaRPM") != -1)
            {
                Series serie = chartCooler.Series["LinhaRPM"];

                
                serie.Points.AddXY(tempoAtual, rpmAtual);

                if (serie.Points.Count > MaxPontosNoGrafico)
                {
                    serie.Points.RemoveAt(0);
                    
                    chartCooler.ChartAreas[0].AxisX.Minimum = tempoAtual - MaxPontosNoGrafico;
                    chartCooler.ChartAreas[0].AxisX.Maximum = tempoAtual;
                }

                if (rpmAtual > chartCooler.ChartAreas[0].AxisY.Maximum)
                {
                    chartCooler.ChartAreas[0].AxisY.Maximum = Math.Ceiling(rpmAtual / 500.0) * 500;
                }
            }
        }
        

        private void ConfigurarControles()
        {
            sliderTemperatura.ValueChanged += SliderTemperatura_ValueChanged;
            timerAtualizacao.Tick += TimerAtualizacao_Tick;
            timerAtualizacao.Start();
        }

        private void ConfigurarGrafico()
        {
            chartTemperatura.Series.Clear();
            chartTemperatura.ChartAreas.Clear();

            ChartArea area = new ChartArea("AreaTemp");
            area.AxisX.Title = "";
            area.AxisY.Title = "";
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 120;
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartTemperatura.ChartAreas.Add(area);

            Series serie = new Series("Temperatura")
            {
                ChartType = SeriesChartType.FastLine,
                BorderWidth = 3,
                Color = Color.Red,
                XValueType = ChartValueType.Int32
            };
            chartTemperatura.Series.Add(serie);
        }

        private void SliderTemperatura_ValueChanged(object sender, EventArgs e)
        {
            
        }

        private async void TimerAtualizacao_Tick(object sender, EventArgs e)
        {
            
            if (_estaAtualizando) return;
            _estaAtualizando = true;

            try
            {
                
                await Task.Run(() =>
                {
                    pc.Accept(new UpdateVisitor());
                });

                tempoDecorridoSegundos++;

                
                AplicarControleFan(sliderTemperatura.Value);

                
                float tempReal = ObterTemperaturaUniversal();
                if (radioButton1.Checked) { 
                lblTemperaturaVal.Text = $"{tempReal:F1} °C";
                }
                else if (radioButton2.Checked)
                {
                    double tempF = (tempReal * 1.8) + 32;
                    lblTemperaturaVal.Text = $"{tempF:F1} °F";
                }
                else if (radioButton3.Checked)
                {
                    double tempK = tempReal + 273.15;
                    lblTemperaturaVal.Text = $"{tempK:F1} °K";
                }

                    var serieTemp = chartTemperatura.Series["Temperatura"];
                serieTemp.Points.AddXY(tempoDecorridoSegundos, tempReal);

                if (serieTemp.Points.Count > MaxPontosNoGrafico)
                {
                    serieTemp.Points.RemoveAt(0);
                    chartTemperatura.ChartAreas[0].AxisX.Minimum = tempoDecorridoSegundos - MaxPontosNoGrafico;
                    chartTemperatura.ChartAreas[0].AxisX.Maximum = tempoDecorridoSegundos;
                }

                
                AtualizarLabelRpm();
                AtualizarExibicaoRpm(tempoDecorridoSegundos);
            }
            catch
            {
                
            }
            finally
            {
                _estaAtualizando = false;
            }
        }

        private void AplicarControleFan(int porcentagem)
        {
           
            if (cmbDispositivos.SelectedItem is ItemControleFan dispositivoSelecionado)
            {
                
                dispositivoSelecionado.SensorControle.Control?.SetSoftware(porcentagem);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string tem = File.ReadAllText(tempet);
            if (tem == "C")
            {
                radioButton1.Checked = true;
                radioButton2.Checked = false;
                radioButton3.Checked = false;
            }
            else if (tem == "F")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = true;
                radioButton3.Checked = false;
            }
            else if (tem == "K")
            {
                radioButton1.Checked = false;
                radioButton2.Checked = false;
                radioButton3.Checked = true;
            }
            string data = File.ReadAllText(cam);
            if(data == "EN")
            {
                potencia = "Power: ";
                dev.Text = "Developed by Dr. Jhonatan";
                label19.Text = "Language";
                label20.Text = "English";
                button1.Text = "Scanner";
                btnResetar.Text = "CPU Control";
                label15.Text = "Cooler Speed";
                label14.Text = "Processor Temperature";
                label16.Text = "Processor\nTemperature";
                label17.Text = "Cooler Speed";
                label12.Text = "Settings";
                button2.Text = "Change Language";
                label18.Text = potencia + cash + "%";
                button3.Text = "Donate to developer";
            }
            else if (data == "ES")
            {
                potencia = "Potencia: ";
                dev.Text = "Desarrollado por Dr. Jhonatan";
                label19.Text = "Idioma";
                label20.Text = "Español";
                button1.Text = "Informe";
                btnResetar.Text = "Devolver el Control";
                label15.Text = "Velocidad del Cooler";
                label14.Text = "Temperatura Actual";
                label16.Text = "Temperatura\nActual";
                label17.Text = "Velocidad del\nCooler";
                label12.Text = "Configuración";
                button2.Text = "Cambiar Idioma";
                label18.Text = potencia + cash + "%";
                button3.Text = "Dona al desarrollador";
            }
            else if (data == "KO")
            {
                potencia = "힘 ";
                dev.Text = "조나단 박사가 개발했습니다.";
                label19.Text = "언어";
                label20.Text = "한국인";
                button1.Text = "세부";
                btnResetar.Text = "CPU로 제어권을 반환합니다.";
                label15.Text = "냉각 속도";
                label14.Text = "현재 온도";
                label16.Text = "현재 온도";
                label17.Text = "냉각 속도";
                label12.Text = "설정";
                button2.Text = "언어 변경";
                label18.Text = potencia + cash + "%";
                button3.Text = "개발자에게 기부하세요";
            }
            else if (data == "JA")
            {
                potencia = "力 ";
                dev.Text = "ジョナサン博士によって開発されました";
                label19.Text = "言語";
                label20.Text = "日本語";
                button1.Text = "報告";
                btnResetar.Text = "CPU制御";
                label15.Text = "冷却速度";
                label14.Text = "プロセッサの温度";
                label16.Text = "プロセッサの温度";
                label17.Text = "冷却速度";
                label12.Text = "設定";
                button2.Text = "言語を変更する";
                label18.Text = potencia + cash + "%";
                button3.Text = "開発者に寄付する";
            }
            else if (data == "CH")
            {
                potencia = "力量 ";
                dev.Text = "由乔纳森博士开发";
                label19.Text = "语言";
                label20.Text = "简体中文";
                button1.Text = "报告";
                btnResetar.Text = "CPU控制";
                label15.Text = "冷却速度";
                label14.Text = "处理器温度";
                label16.Text = "处理器温度";
                label17.Text = "冷却速度";
                label12.Text = "设置";
                button2.Text = "更改语言";
                label18.Text = potencia + cash + "%";
                button3.Text = "向开发者捐款";
            }
            else if (data == "DE")
            {
                potencia = "Leistung: ";
                dev.Text = "Entwickelt von Dr. Jhonatan";
                label19.Text = "Sprache";
                label20.Text = "Deutsch";
                button1.Text = "Bericht";
                btnResetar.Text = "CPU-Steuerung";
                label15.Text = "Kühlergeschwindigkeit";
                label14.Text = "Aktuelle Temperatur";
                label16.Text = "Aktuelle\nTemperatur";
                label17.Text = "Kühle\nGeschwindigkeit";
                label12.Text = "Einstellungen";
                button2.Text = "Sprache ändern";
                label18.Text = potencia + cash + "%";
                button3.Text = "Spenden Sie an den Entwickler";
            }
            else if (data == "ITA")
            {
                potencia = "Potenza: ";
                dev.Text = "Sviluppato dal Dott. Jhonatan";
                label19.Text = "Lingua";
                label20.Text = "Italiano";
                button1.Text = "Rapporto";
                btnResetar.Text = "Controllo CPU";
                label15.Text = "Velocità più Fredda";
                label14.Text = "Temperatura Attuale";
                label16.Text = "Temperatura\nAttuale";
                label17.Text = "Velocità più\nFredda";
                label12.Text = "Impostazioni";
                button2.Text = "Cambia Lingua";
                label18.Text = potencia + cash + "%";
                button3.Text = "Fai una donazione allo sviluppatore";
            }
            else if (data == "FR")
            {
                potencia = "Pouvoir: ";
                dev.Text = "Développé par le Dr Jhonatan";
                label19.Text = "Langue";
                label20.Text = "Français";
                button1.Text = "Rapport";
                btnResetar.Text = "Contrôle du Processeur";
                label15.Text = "Vietesse du Refroidisseur";
                label14.Text = "Température Actualle";
                label16.Text = "Température\nActualle";
                label17.Text = "Vietesse du\nRefroidisseur";
                label12.Text = "Paramètres";
                button2.Text = "Changer de Langue";
                label18.Text = potencia + cash + "%";
                button3.Text = "Faites un don au développeur";
            }
            else
            {
                potencia = "Potência: ";
                dev.Text = "Desenvolvido por Dr. Jhonatan";
                label19.Text = "Idioma";
                label20.Text = "Português";
                button1.Text = "Relatório";
                btnResetar.Text = "Restaurar";
                label15.Text = "Velocidade Cooler";
                label14.Text = "Temperatura Atual";
                label16.Text = "Temperatura\nAtual";
                label17.Text = "Velocidade\nCooler";
                label12.Text = "Configurações";
                button2.Text = "Mudar Idioma";
                label18.Text = potencia + cash + "%";
                button3.Text = "Doar para desenvolvedor";
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            
            foreach (IHardware hardware in pc.Hardware)
            {
                foreach (ISensor sensor in hardware.Sensors)
                {
                    if (sensor.SensorType == SensorType.Control)
                        sensor.Control?.SetDefault();
                }
            }

            pc?.Close();
            base.OnFormClosed(e);
        }
        private void ConfigurarGraficoRpm()
        {
            chartCooler.Series.Clear();

            Series serieCooler = chartCooler.Series.Add("LinhaRPM");
            serieCooler.ChartType = SeriesChartType.FastLine;
            serieCooler.Color = Color.BlueViolet; 
            serieCooler.BorderWidth = 2;

            
            ChartArea area = chartCooler.ChartAreas[0];
            area.AxisY.Minimum = 0;
            area.AxisY.Maximum = 2500;
        }
        private void sliderTemperatura_Scroll(object sender, EventArgs e) 
        {
            string data = File.ReadAllText(cam);
            if (sliderTemperatura.Value == 20)
            {

                if (data == "EN")
                {
                    MessageBox.Show("Some modern CPUs prevent the cooling fan speed from being reduced below 20% of its power. However, we do not recommend lowering it, and we accept no responsibility for any issues that may arise from reducing the power to less than 20%.", "Stern Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
                else if (data == "ES")
                {
                    MessageBox.Show("Algunos procesadores modernos impiden que la velocidad del disipador se reduzca por debajo del 20% de su potencia. Sin embargo, no recomendamos reducirla aún más y no nos hacemos responsables de los problemas que pueda surgir al reducir la potencia por debajo del 20%.", "Severa Advetencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);

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
                    MessageBox.Show("Algumas CPUs modernas impedem a redução da velocidade do cooler a menos que 20% de sua potência. Porém não recomendamos que você diminua, não nos responsabilizamos por possíveis problemas que você possa obter ao reduzir a potência a um número menor que 20%", "Aviso Severo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            ptg = sliderTemperatura.Value;
            cash = ptg.ToString();
            label18.Text = potencia + cash + "%";
        
        }

        private float ObterRpmFan()
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
            string data = File.ReadAllText(cam);
            var relatorio = new System.Text.StringBuilder();

            if (data == "EN")
            {
                relatorio.AppendLine("=== COMPLETE SENSOR REPORT ===\n");
            }
            else if (data == "ES")
            {
                relatorio.AppendLine("=== INFORME COMPLETO DEL SENSOR ===\n");
            }
            else if (data == "KO")
            {
                relatorio.AppendLine("=== 센서 종합 보고서 ===\n");
            }
            else if (data == "JA")
            {
                relatorio.AppendLine("=== センサーレポート全文 ===\n");
            }
            else if (data == "DE")
            {
                relatorio.AppendLine("=== VOLLSTÄNDIGER SENSORBERICHT ===\n");
            }
            else if (data == "ITA")
            {
                relatorio.AppendLine("=== REPORT COMPLETO DEL SENSORE ===\n");
            }
            else if (data == "FR")
            {
                relatorio.AppendLine("=== RAPPORT COMPLET SUR LES CAPTEURS ===\n");
            }
            else if (data == "CH")
            {
                relatorio.AppendLine("=== 完整传感器报告 ===\n");
            }
            else
            {
                relatorio.AppendLine("=== RELATÓRIO COMPLETO DE SENSORES ===\n");
            }


            foreach (IHardware hw in pc.Hardware)
            {
                hw.Update();
                relatorio.AppendLine($"[ {hw.Name} ] ({hw.HardwareType})");

                
                var sensoresAgrupados = hw.Sensors.GroupBy(s => s.SensorType);

                foreach (var grupo in sensoresAgrupados)
                {
                    relatorio.AppendLine($"  -- Type: {grupo.Key} --");
                    foreach (ISensor sensor in grupo)
                    {
                        if (sensor.Value.HasValue)
                        {
                            relatorio.AppendLine($"     • {sensor.Name}: {sensor.Value.Value:F1}");
                        }
                    }
                }

                
                foreach (IHardware subHw in hw.SubHardware)
                {
                    subHw.Update();
                    relatorio.AppendLine($"\n  [ Sub-Hardware: {subHw.Name} ]");

                    foreach (ISensor subSensor in subHw.Sensors)
                    {
                        if (subSensor.Value.HasValue)
                        {
                            relatorio.AppendLine($"     • {subSensor.SensorType} | {subSensor.Name}: {subSensor.Value.Value:F1}");
                        }
                    }
                }

                relatorio.AppendLine(new string('-', 40));
            }

            
            ExibirJanelaLog(relatorio.ToString());
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
        private void AtualizarLabelRpm()
        {
            if (cmbDispositivos.SelectedItem is ItemControleFan dispositivoSelecionado)
            {
                
                if (dispositivoSelecionado.SensorRpm != null && dispositivoSelecionado.SensorRpm.Value.HasValue)
                {
                    lblRpm.Text = $"{dispositivoSelecionado.SensorRpm.Name}: {dispositivoSelecionado.SensorRpm.Value.Value:F0} RPM";
                    return;
                }
            }

            lblRpm.Text = "Cooler: 0 RPM";
        }
        private void CarregarDispositivosFan()
        {
            cmbDispositivos.Items.Clear();

            foreach (IHardware hardware in pc.Hardware)
            {
                
                var controles = hardware.Sensors.Where(s => s.SensorType == SensorType.Control).ToList();
                var leitoresRpm = hardware.Sensors.Where(s => s.SensorType == SensorType.Fan).ToList();

                for (int i = 0; i < controles.Count; i++)
                {
                    
                    ISensor sensorRpmCorrespondente = (i < leitoresRpm.Count) ? leitoresRpm[i] : null;

                    cmbDispositivos.Items.Add(new ItemControleFan
                    {
                        NomeExibicao = $"{hardware.Name} - {controles[i].Name}",
                        HardwarePai = hardware,
                        SensorControle = controles[i],
                        SensorRpm = sensorRpmCorrespondente
                    });
                }
            }

            if (cmbDispositivos.Items.Count > 0)
            {
                cmbDispositivos.SelectedIndex = 0;
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void btnResetar_Click(object sender, EventArgs e)
        {
            string data = File.ReadAllText(cam);
            foreach (var hw in pc.Hardware)
    {
                foreach (var s in hw.Sensors.Where(x => x.SensorType == SensorType.Control))
                {
                    s.Control?.SetDefault(); 
                }
            }
            sliderTemperatura.Value = 25;
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
            string data = File.ReadAllText(cam);
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
            float tempReal = ObterTemperaturaUniversal();
            double tempF = (tempReal * 1.8) + 32; 
            lblTemperaturaVal.Text = $"{tempF:F1} °F";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form3 form = new Form3();
            form.Show();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            File.WriteAllText(tempet, "C");
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            File.WriteAllText(tempet, "K");
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
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }
    public class ItemControleFan
    {
        public string NomeExibicao { get; set; }
        public IHardware HardwarePai { get; set; }
        public ISensor SensorControle { get; set; } 
        public ISensor SensorRpm { get; set; }      

        public override string ToString()
        {
            return NomeExibicao;
        }
    }
}