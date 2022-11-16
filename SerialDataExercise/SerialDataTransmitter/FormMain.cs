using SerialDataBase;
using System;
using System.Windows.Forms;

namespace SerialDataTransmitter
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private async void buttonTransmit_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            try
            {
                button.Enabled = false;

                using (SerialPortAccessLayer serialPort = new SerialPortAccessLayer(1))
                {
                    Console.WriteLine("");
                    Console.WriteLine("COM1 Open.");
                    Console.WriteLine("Transmitting...");
                    Console.WriteLine("");

                    byte[] packet = new byte[SerialPortAccessLayer.PACKET_SIZE];
                    
                    // Status flags
                    packet[0] = 0xad;

                    // Little-endian encoded signed 16-bit temperature in degrees C
                    packet[1] = 0xfe;
                    packet[2] = 0xff;

                    // Single-precision 32-bit floating-point DC voltage
                    byte[] voltBytes = BitConverter.GetBytes(13.2f);
                    
                    Array.Copy(voltBytes, 0, packet, 3, voltBytes.Length);

                    // Two-digit decimal major firmware version,
                    // two-digit decimal minor firmware version,
                    // ASCI alphatical release candidate version
                    packet[7] = 0x02;
                    packet[8] = 0x05;
                    packet[9] = 0x63;

                    int index = 10;

                    // Null-terminated ASCII string message to the user
                    foreach (char character in "Hello, World!\0")
                        packet[index++] = (byte)character;

                    await serialPort.transmitAsync(packet);
                }
            }

            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }

            finally
            {
                button.Enabled = true;
            }
        }
    }
}
