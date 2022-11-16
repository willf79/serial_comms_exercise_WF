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
                    int index = 0;
 
                    // Status flags
                    packet[index++] = 0xad;

                    // Little-endian encoded signed 16-bit temperature in degrees C
                    packet[index++] = 0xfe;
                    packet[index++] = 0xff;

                    // Single-precision 32-bit floating-point DC voltage
                    Buffer.BlockCopy(BitConverter.GetBytes(13.2f), 0, packet, index++, 4);

                    // Two-digit decimal major firmware version,
                    // two-digit decimal minor firmware version,
                    // ASCI alphatical release candidate version
                    packet[index++] = 0x02;
                    packet[index++] = 0x05;
                    packet[index++] = 0x63;

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
