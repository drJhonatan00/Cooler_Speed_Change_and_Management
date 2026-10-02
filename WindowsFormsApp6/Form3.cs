using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form3 : Form
    {
        string cam = @"WindowsForms6.config";
        public Form3()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Clicked(object sender, EventArgs e)
        {
            panel3.BackColor = Color.DarkGray;
        }

        private void Clicked(object sender, EventArgs e)
        {

            panel3.BackColor = Color.RosyBrown;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            string data = File.ReadAllText(cam);
            if (data == "EN")
            {
                label4.Text = "Thank You!";
                label2.Text = "Scan this QR-Code";
                label3.Text = "You choose the value";
            }
            else if (data == "ES")
            {
                label4.Text = "¡Gracias!";
                label2.Text = "Escanee el QR Code";
                label3.Text = "Tú eliges el valor";
            }
            else if (data == "KO")
            {
                label4.Text = "매우 감사합니다";
                label2.Text = "QR 코드를 스캔하세요.";
                label3.Text = "값을 직접 선택하세요";
            }
            else if (data == "JA")
            {
                label4.Text = "どうもありがとうございます";
                label2.Text = "QRコードをスキャンしてください";
                label3.Text = "値はあなたが決めます。";
            }
            else if (data == "CH")
            {
                label4.Text = "非常感谢";
                label2.Text = "扫描二维码";
                label3.Text = "你来选择数值。";
            }
            else if (data == "DE")
            {
                label4.Text = "Vielen Dank!";
                label2.Text = "Scannen Sie den QR-Code";
                label3.Text = "Sie bestimmen den Wert";
            }
            else if (data == "ITA")
            {
                label4.Text = "Grazie mille!";
                label2.Text = "Scansiona il QR-Code";
                label3.Text = "Tu scegli il valore";
            }
            else if (data == "FR")
            {
                label4.Text = "Merci!";
                label2.Text = "Scannez le QR-Code";
                label3.Text = "Vous choisissez la valeur.";
            }
            else
            {
                label4.Text = "Obrigado!";
                label2.Text = "Escaneie o QR Code";
                label3.Text = "Você escolhe o valor";
            }
        }
    }
}


//Watashi wa watashi sore dake