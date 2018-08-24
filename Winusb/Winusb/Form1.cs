using MadWizard.WinUSBNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using System.Diagnostics;
using System.Threading;

namespace Winusb
{
    public partial class Form1 : Form
    {
        USBDevice device;
        Guid deviceGUID = new Guid("{5D087431-5725-4673-83A0-4394EC2F0EEB}");
        Logger logger = NLog.LogManager.GetCurrentClassLogger();
        USBNotifier connectionNotifier;
        public Form1()
        {
            InitializeComponent();
            logger.Info("Trying to connect to device with GUID: {0}", deviceGUID.ToString());
            if (USBDevice.GetDevices(deviceGUID).Count() > 0)
            {
                try
                {
                    device = USBDevice.GetSingleDevice(deviceGUID);
                }catch(USBException ex)
                {
                    logger.Warn("Something went wrong connecting to device", ex);
                }
            }

            connectionNotifier = new USBNotifier(this, deviceGUID); //setup connection and disconnection Notifier
            if (device == null)
            {
                logger.Warn("No Device found");
            }
            else
            {
                logger.Info("Connected to device");
                logDeviceiFaceProperties(device);
            }
            connectionNotifier.Arrival += ConnectionNotifier_Arrival;
            connectionNotifier.Removal += ConnectionNotifier_Removal;

        }

        private void logDeviceiFaceProperties(USBDevice device)
        {
            logger.Trace("Device Properties:");
            logger.Trace(device.Descriptor.FullName);
            logger.Trace("VID: {0:X4} - PID: {1:X4} ", device.Descriptor.VID,device.Descriptor.PID);
            logger.Trace("MFG {0} ",device.Descriptor.Manufacturer);
            logger.Trace("SerialNo {0} ", device.Descriptor.SerialNumber);
            logger.Trace("Product {0} ", device.Descriptor.Product);
            logger.Trace("Number of interfaces: {0}", device.Interfaces.Count());
            int count = 0;
            foreach (USBInterface iface in device.Interfaces)
            {
                logger.Trace("Interface {0}", count);

                logger.Trace("Number of Pipes in current interface: {0} ", iface.Pipes.Count());
                count++;
                int pipecount = 0;
                foreach (USBPipe pipe in iface.Pipes)
                {

                    logger.Trace("Pipe {0} with direction {1}", pipecount, pipe.IsIn ? "IN" : "OUT");
                    pipecount++;
                }

            }

        }

        private void ConnectionNotifier_Removal(object sender, USBEvent e)
        {
            logger.Warn("Device Disconnected");
        }


        //Used to detect arrival of usb device with right GUID
        private void ConnectionNotifier_Arrival(object sender, USBEvent e)
        {
            device = USBDevice.GetSingleDevice(deviceGUID);
            logger.Info("New USB device mounted");
            if (device == null)
            {
                logger.Warn("No Device found");
            }
            else
            {
                logger.Info("Connected to device");
                logDeviceiFaceProperties(device);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (device != null)
            {
                byte[] data = Encoding.ASCII.GetBytes(tb_sendData.Text); //encode text as ascii
                byte[] rcvData = new byte[data.Length]; //generate reception buffer
                Stopwatch sw = Stopwatch.StartNew(); 
                device.Interfaces[0].OutPipe.Write(data); //write data
                device.Interfaces[0].InPipe.Read(rcvData); //get data from loopback

                sw.Stop();
                double ticks = sw.ElapsedTicks;
                double seconds = ticks / Stopwatch.Frequency;
                double microseconds = (ticks / Stopwatch.Frequency) * 1000000;
                logger.Trace("RoundTripTime for {0} bytes is {1} ", data.Length, microseconds);

                //Write data to textbox
                richTextBoxReceived.AppendText(Encoding.ASCII.GetString(rcvData));
                richTextBoxReceived.AppendText("\r\n");
                richTextBoxReceived.SelectionStart = richTextBoxReceived.Text.Length;
                // scroll it automatically
                richTextBoxReceived.ScrollToCaret();
            }
        }


        //Small function to benchmark performance, not used
        public void benchmark(int packetSize, int experimentLength) { 
            if (device != null)
            {
                RandomBufferGenerator(50000); //prime buffer for random buffer generator

                
                double[] times = new double[experimentLength];
                double[] timesSeconds = new double[experimentLength];


                for (int i = 0; i < experimentLength; i++)
                {
                    byte[] data = GenerateBufferFromSeed(packetSize);
                    byte[] rcvData = new byte[data.Length];
                    Stopwatch sw = Stopwatch.StartNew();
                    device.Interfaces[0].InPipe.Flush();
                    device.Interfaces[0].OutPipe.Write(data);
                    device.Interfaces[0].InPipe.Read(rcvData);

                    sw.Stop();
                    double ticks = sw.ElapsedTicks;
                    double microseconds = (ticks / Stopwatch.Frequency) * 1000000;

                    bool isEqual = Enumerable.SequenceEqual(data, rcvData);
                    if (!isEqual) throw new Exception("Sent data is not equal to received data");

                    times[i] = microseconds;
                    timesSeconds[i] = (ticks / Stopwatch.Frequency);
                    Thread.Sleep(10);
                }

                logger.Trace("Average RoundTripTime for {0} packets of {1} bytes is {2} ", experimentLength, packetSize, times.Average());
                logger.Trace("Timeseconds avg {0} ", timesSeconds.Average());
                logger.Trace("Average transfer rate: {0} kbps", packetSize*8/2 / (timesSeconds.Average())/1000);

            }



        }


        private  Random _random = new Random();
        private  byte[] _seedBuffer;

        public void RandomBufferGenerator(int maxBufferSize)
        {
            _seedBuffer = new byte[maxBufferSize];

            _random.NextBytes(_seedBuffer);
        }

        public byte[] GenerateBufferFromSeed(int size)
        {
            int randomWindow = _random.Next(0, size);

            byte[] buffer = new byte[size];

            Buffer.BlockCopy(_seedBuffer, randomWindow, buffer, 0, size - randomWindow);
            Buffer.BlockCopy(_seedBuffer, 0, buffer, size - randomWindow, randomWindow);

            return buffer;
        }



    }
}
