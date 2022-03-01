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
    public partial class Form1 : Form
    {
        string serialDataIn;
        int contador;

        Boolean lockScroll = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button_open.Enabled = true;
            button_close.Enabled = false;
            button_send.Enabled = false;
            progressBar_statusBar.Value = 0;
            label_status.Text = "DESCONECTADO";
            label_status.ForeColor = Color.Red;

            comboBox_baudRate.Text = "9600";
            string[] portLists = SerialPort.GetPortNames();
            comboBox_comPort.Items.AddRange(portLists);

            //var thread1 = new Thread(() => ttOne());
            //thread1.Start();


            //Console.WriteLine("Teste");





        }

        /*private static void callback(object state)
        {
            Console.WriteLine(state);
        }*/

        /*public void ttOne() {
            for (int i = 1; i < 1001; i++)
                Console.WriteLine($"Thread 2 -> Iteração {i}");

        }*/

        private void button_open_Click(object sender, EventArgs e)
        {
            try
            {
                contador = 1;
                dataGridView1.Rows.Clear();

                Thread _readThread = new Thread(Read);
                serialPort1.PortName = comboBox_comPort.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox_baudRate.Text);
                serialPort1.Parity = Parity.None;
                serialPort1.DataBits = 8;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Handshake = Handshake.None;
                serialPort1.DtrEnable = true;
                _readThread.Start();


                button_open.Enabled = false;
                button_close.Enabled = true;
                button_send.Enabled = true;
                progressBar_statusBar.Value = 100;
                label_status.Text = "CONECTADO";
                label_status.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Read() {
            
            serialPort1.Open();
        }

        private void button_close_Click(object sender, EventArgs e)
        {


            if (serialPort1.IsOpen) {
                try
                {
                    serialPort1.DtrEnable = false;
                    serialPort1.RtsEnable = false;
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                    serialPort1.Close();
                    button_open.Enabled = true;
                    button_close.Enabled = false;
                    button_send.Enabled = false;
                    progressBar_statusBar.Value = 0;
                    label_status.Text = "DESCONECTADO";
                    label_status.ForeColor = Color.Red;
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                } 
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen) {
                try
                {
                    serialPort1.DtrEnable = false;
                    serialPort1.RtsEnable = false;
                    serialPort1.DiscardInBuffer();
                    serialPort1.DiscardOutBuffer();
                    serialPort1.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button_send_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.Write(textBox_textSent.Text + "#");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            //Console.WriteLine(serialDataIn);

            int n = dataGridView1.Rows.Add();
            dataGridView1.Rows[n].Cells[0].Value = contador++;
            dataGridView1.Rows[n].Cells[1].Value = serialDataIn;


            /*if (lockScroll)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            }*/

            //System.Threading.Timer timer = new System.Threading.Timer(callback, serialDataIn, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
            //Thread.Sleep(1000); // Wait a bit over 4 seconds.



        }


        protected override void WndProc(ref Message m)
        {

            //Console.WriteLine("XX: " + serialPort1.IsOpen);
            // Call back to the original first
            base.WndProc(ref m);

            // You'll receive a lot more messages than just device change notifications
            switch (m.Msg)
            {
                // https://wiki.winehq.org/List_Of_Windows_Messages
                // The numeric value of WM_DEVICE_CHANGED
                case 0x0219:
                    // Inside the device changed, you'll need to know what to do with m.WParam
                    // https://docs.microsoft.com/en-us/windows/win32/winmsg/about-messages-and-message-queues?redirectedfrom=MSDN#system-defined-messages
                    
                    try
                    {

                        Console.WriteLine("XX: " + serialPort1.IsOpen);
                        button_open.Enabled = true;
                        button_close.Enabled = false;
                        button_send.Enabled = false;
                        progressBar_statusBar.Value = 0;
                        label_status.Text = "DESCONECTADO";
                        label_status.ForeColor = Color.Red;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    break;
                default:
                    break;
            }
        }
        private void richTextBox_textReceiver_TextChanged(object sender, EventArgs e)
        {
            //richTextBox_textReceiver.SelectionStart = richTextBox_textReceiver.Text.Length;
            //richTextBox_textReceiver.ScrollToCaret();

        }

        private void comboBox_comPort_Click(object sender, EventArgs e)
        {
            Console.WriteLine("comboBox_comPort_Click");
            string[] portLists = SerialPort.GetPortNames();

            comboBox_comPort.Items.Clear();
            comboBox_comPort.ResetText();
            comboBox_comPort.Items.AddRange(portLists);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            lockScroll = checkBox1.Checked;
     
            if (lockScroll)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
            }

        }




        private void btnComunicacao_Click(object sender, EventArgs e)
        {

            Comunicacao fr = new Comunicacao();
            fr.ShowDialog();
        }

        private void serialPort1_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Console.WriteLine("OK1");
        }

        private void serialPort1_PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            Console.WriteLine("OK2");
        }

    }
}
