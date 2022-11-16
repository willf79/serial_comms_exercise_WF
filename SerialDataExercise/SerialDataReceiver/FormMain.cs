using SerialDataBase;
using System;
using System.Collections;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Remoting.Metadata;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;
using static System.Resources.ResXFileRef;

namespace SerialDataReceiver
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private async void buttonReceive_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;

            try
            {
                button.Enabled = false;
                
                //Receive byte packet via COM2 and assign to var rPacket
                using (SerialPortAccessLayer serialPort = new SerialPortAccessLayer(2))
                {
                    Console.WriteLine(" ");
                    
                    // Check how the computer architecture is processing bytes (little-endian is most common for PCs)
                    bool endianness = BitConverter.IsLittleEndian;
                    Console.WriteLine("Computer uses little-endian? " + endianness);

                    Console.WriteLine(" ");
                    Console.WriteLine("COM2 Open.");
                    Console.WriteLine("Receiving...");
                    Console.WriteLine(" ");

                    //Initialize string array to hold output values
                    string[] output = new string[12];
                    
                    //Current Directory = serial_comms_exercise\SerialDataExercise\SerialDataReceiver\bin\Debug
                    string filePath = Path.Combine(Environment.CurrentDirectory, "Report.txt");

                    //Receive serial data via COM2 serial port
                    byte[] rPacket = await serialPort.receiveAsync();

                    Console.WriteLine(" ");
                    Console.WriteLine("Serial Data Received.");
                    Console.WriteLine(" ");

                    // Fields 1-8, Byte Offset 0: Bit flags 1-8.
                    var bitstring = Convert.ToString(rPacket[0], 2);
                    
                    for (int i = 0; i < bitstring.Length; i++)
                    {
                        Console.WriteLine("Field " + (i+1).ToString() + ":");
                        
                        var flag = bitstring.Substring(i, 1);
                        
                        if (flag == "0")
                        {
                            Console.WriteLine("False");
                            output[i] = "Field " + (i + 1) + ": Bit flag is False.";
                        }
                        
                        if (flag == "1")
                        {
                            Console.WriteLine("True");
                            output[i] = "Field " + (i + 1) + ": Bit flag is True.";
                        }
                        
                        Console.WriteLine(" ");
                    }

                    // Field 9, Byte Offset 1-2: 16-bit signed integer ambient temperature in degrees C stored in little-endian order.
                    Console.WriteLine("Field 9:");

                    byte[] tempPacket = new byte[2];
                    Array.Copy(rPacket, 1, tempPacket, 0, 2);
                    
                    if (!endianness)
                    {
                        Array.Reverse(tempPacket);
                    }
                    
                    var f9 = BitConverter.ToInt16(tempPacket, 0);
                    
                    Console.WriteLine(f9);
                    output[8] = "Field 9: " + f9 + " degrees C";


                    Console.WriteLine(" ");


                    //Field 10, Byte Offset 3 - 6: 32 - bit double-precision floating - point numeric voltage.
                    Console.WriteLine("Field 10:");

                    byte[] voltPacket = new byte[8];
                    Array.Copy(rPacket, 3, voltPacket, 0, 4);
                    
                    if (endianness)
                    {
                        Array.Reverse(voltPacket);
                    }
                    
                    var f10 = BitConverter.ToDouble(voltPacket, 0);
                    
                    Console.WriteLine(f10);
                    output[9] = "Field 10: " + f10 + " volts";


                    Console.WriteLine(" ");


                    ////Field 11, Byte Offset 7-9: 8-bit numeric major software version, 8-bit numeric minor software version, and an alphabetical release candidate ASCII character.
                    Console.WriteLine("Field 11:");

                    byte maVersion = rPacket[7];
                    byte miVersion = rPacket[8];
                    
                    //The alphabetical release candidate ASCII character is a lowercase L (hard to tell between 1 and l)
                    String releaseASCII = System.Text.Encoding.ASCII.GetString(rPacket, 9, 1);

                    //Combine versions into one
                    string f11 = maVersion + "." + miVersion + "." + releaseASCII;
                    
                    Console.WriteLine(f11);
                    output[10] = "Field 11: Software version " + f11;


                    Console.WriteLine(" ");


                    //Field 12, Byte Offset 10-23: 14-character null-terminated ASCII string including terminator.
                    Console.WriteLine("Field 12:");

                    String messageASCII = System.Text.Encoding.ASCII.GetString(rPacket, 7, 13);
                    
                    Console.WriteLine(messageASCII);
                    output[11] = "Field 12: " + messageASCII;


                    Console.WriteLine(" ");


                    System.IO.File.WriteAllLines(@filePath, output);
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
