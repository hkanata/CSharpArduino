using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
namespace ArduinoCSharp
{
    public partial class Comunicacao : Form
    {

        string serialDataIn;
        public Comunicacao()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                Thread _readThread = new Thread(Read);
                serialPort1.PortName = "COM4";
                serialPort1.BaudRate = Convert.ToInt32("9600");
                serialPort1.Parity = Parity.None;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Handshake = Handshake.None;
                serialPort1.DtrEnable = true;
                _readThread.Start();


                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Read()
        {

            serialPort1.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();
       
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Comunicacao_FormClosing(object sender, FormClosingEventArgs e)
        {
            Console.WriteLine("a2");

  
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            serialDataIn = serialPort1.ReadExisting();




            this.Invoke(new EventHandler(ShowData));
        }

        private void ShowData(object sender, EventArgs e)
        {
            //richTextBox_textReceiver.Text += serialDataIn;
            Console.WriteLine(serialDataIn);

           

            //System.Threading.Timer timer = new System.Threading.Timer(callback, serialDataIn, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            //Thread.Sleep(1000); // Wait a bit over 4 seconds.



        }

        private void Comunicacao_FormClosed(object sender, FormClosedEventArgs e)
        {
            Console.WriteLine("a1");
            if (serialPort1.IsOpen)
            {
                try
                {

                    serialPort1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
