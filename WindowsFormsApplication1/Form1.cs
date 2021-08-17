using System;
using System.IO;
using System.IO.Ports;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using NationalInstruments.VisaNS;

                        
namespace WindowsFormsApplication1
{

    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void setResi(string seriNum)
        {
            try
            {
                serialPort1.Open();
                Thread.Sleep(10);
                serialPort1.Write(seriNum);
                serialPort1.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void serialReset()
        {
            string[] ptNames = SerialPort.GetPortNames();

            try
            {
                serialPort1.PortName = ptNames[0];
                serialPort1.Open();
                Thread.Sleep(3000);
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonCalc_Click(object sender, EventArgs e)
        {  
            string tb1, tb2;
            double tmpr1, tmpr2, data1, data2 ;

            double[] tb3a = new double[20];
            double[] tb3b = new double[20];

            richTextBox3.Text = "";

            for (int i = 0; i < tb3a.Length; i++)
            {
                tb1 = richTextBox1.Text.Split(',')[i];
                data1 = double.Parse(tb1.Split('@')[0]);
                tmpr1 = double.Parse(tb1.Split('@')[1]);

                if (checkBox1.Checked == true)
                {
                    tb2 = richTextBox2.Text.Split(',')[i];
                    data2 = double.Parse(tb2.Split('@')[0]);
                    tmpr2 = double.Parse(tb2.Split('@')[1]);

                    tb3a[i] = (data1 - data2) / (tmpr1 - tmpr2);
                    tb3b[i] = data1 - tb3a[i] * tmpr1;
                }
                else
                {
                    tb3a[i] = 0;
                    tb3b[i] = data1;
                }

                if (tb3a[i] < 0)
                {
                    if (tb3b[i] < 0)
                        richTextBox3.Text += "-" + (-tb3a[i] * 1000).ToString("000000000") + "-" + (-tb3b[i]).ToString("000000000") + "\n" ;
                    else
                        richTextBox3.Text += "-" + (-tb3a[i] * 1000).ToString("000000000") + "+" + tb3b[i].ToString("000000000") + "\n";
                }
                else
                {
                    if (tb3b[i] < 0)
                        richTextBox3.Text += "+" + (tb3a[i] * 1000).ToString("000000000") + "-" + (-tb3b[i]).ToString("000000000") + "\n";
                    else
                        richTextBox3.Text += "+" + (tb3a[i] * 1000).ToString("000000000") + "+" + tb3b[i].ToString("000000000") + "\n";
                }
            }

        }

        private void buttonFile_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            string savedStr;

            ofd.InitialDirectory = @"C:\Users\denshi\Documents\caldata";
            if (ofd.ShowDialog() == DialogResult.OK)
            {

                savedStr = File.ReadAllText(ofd.FileName);

                if (sender == button1)
                {
                    textBox1.Text = Path.GetFileName(ofd.FileName);
                    richTextBox1.Text = savedStr;
                }
                if (sender == button2)
                {
                    textBox2.Text = Path.GetFileName(ofd.FileName);
                    richTextBox2.Text = savedStr;
                }

            }
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            serialReset();

            string UpStr = "C" + richTextBox3.Text.Replace("\n", "");

            setResi(UpStr);

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked == true)
            {
                button2.Enabled = true;
                textBox2.Enabled = true;
                richTextBox2.Enabled = true;
            }
            else
            {
                button2.Enabled = false;
                textBox2.Enabled = false;
                richTextBox2.Enabled = false;
            }
        }

    }
}
