using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ComPort
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private String[] ports;
        private string _parity,_stopbits;
        private int[] _databits = {5,6,7,8};
                          

        private void Form1_Load(object sender, EventArgs e)
        {
            ports = System.IO.Ports.SerialPort.GetPortNames();          // get available ports
            cmbPort.Items.AddRange(ports);
            cmbPort.SelectedIndex = 0;
           
            // set baud rate

            cmbBaudRate.SelectedIndex = 5; //9600

            #region  get parity names & add to combobox.parity

                foreach (string str in Enum.GetNames(typeof(System.IO.Ports.Parity)))
                {
                    (cmbParity).Items.Add(str);
                }

                cmbParity.SelectedIndex = 0;
                _parity = cmbParity.Text;

            #endregion

            #region  get Stop bits names & add to combobox.stop bits

                foreach (string str in Enum.GetNames(typeof(System.IO.Ports.StopBits)))
                {
                    (cmbStopBits).Items.Add(str);
                }

                cmbStopBits.SelectedIndex = 1;
                _stopbits = cmbStopBits.Text;

            #endregion

            #region data bits   

                for (int i = 0; i < _databits.Length; i++)
                {
                    cmbDataBits.Items.Add(Convert.ToString(_databits[i]));
                }

                cmbDataBits.SelectedText = "8";                          //selected quantity bits
                
            #endregion

            timer1.Interval = 100;
            timer1.Enabled = true;
            timer1.Start();

            btnClosePorts.Enabled = false;                               // the button "Close port" isn't enabled
        }

        private void button1_Click(object sender, EventArgs e)
        {
            btnOpenPorts.Enabled = false;
            btnClosePorts.Enabled = true;

            try
             {
                serialPort1.PortName = cmbPort.Text;                                                                       //PortName
                serialPort1.BaudRate = Convert.ToInt32(cmbBaudRate.Text);                                                  //Baud Rate
                serialPort1.Parity = (System.IO.Ports.Parity)Enum.Parse(typeof(System.IO.Ports.Parity),_parity);           //Parity
                serialPort1.StopBits = (System.IO.Ports.StopBits)Enum.Parse(typeof(System.IO.Ports.StopBits), _stopbits);  //StopBits
                serialPort1.DataBits = Convert.ToInt32(cmbDataBits.Text);                                                  //Data bits                                                        //DataBits
                serialPort1.Open();
             }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
              if (serialPort1.IsOpen)
                 {
                     serialPort1.WriteLine(txtMessage.Text + Environment.NewLine);
                     txtMessage.Clear();
                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void btnClosePorts_Click(object sender, EventArgs e)
        {
            btnOpenPorts.Enabled = true;
            btnClosePorts.Enabled = false;

            try
            {
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnReceive_Click(object sender, EventArgs e)
        {
            try
            {
                if (serialPort1.IsOpen)
                {
                    txtReceive.Text = serialPort1.ReadExisting();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } 
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen)
             {
                 serialPort1.Close();
             }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ports = System.IO.Ports.SerialPort.GetPortNames();  // get available ports

            if (serialPort1.IsOpen)
            {
                progressBar1.Value = 100;
                label9.Text = cmbPort.Text + " is opened";
                groupBox1.Enabled = false;
            }
            else
            {
                label9.Text = cmbPort.Text + " is closed";
                progressBar1.Value = 0;
                groupBox1.Enabled = true;
            }
        }

        private void cmbPort_DropDown(object sender, EventArgs e)
        {
            if (cmbPort.Items.Count != ports.Length)
            {
                cmbPort.Items.Clear();
                cmbPort.Items.AddRange(ports);
                cmbPort.Sorted = true;
            }
        }

        private void cmbParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            _parity = cmbParity.Text;
        }

        private void cmbStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            _stopbits = cmbStopBits.Text;
        } 
    }
}

          