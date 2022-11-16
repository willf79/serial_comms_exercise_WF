using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading.Tasks;

namespace SerialDataBase
{
    /// <summary>
    /// Transmit and receive data packets from a serial COM port.
    /// </summary>
    internal class SerialPortAccessLayer : IDisposable
    {
        /// <summary>
        /// Required data packet size in number of bytes.
        /// </summary>
        internal const int PACKET_SIZE = 24;
        
        /// <summary>
        /// Buffer polling duration in milliseconds.
        /// </summary>
        private const int POLLING_DURATION = 10;

        /// <summary>
        /// COM port interface.
        /// </summary>
        private SerialPort _serialPort;
        /// <summary>
        /// Incoming bytes.
        /// </summary>
        private ConcurrentQueue<byte> _rxBuffer;
        /// <summary>
        /// Incoming byte packets.
        /// </summary>
        private ConcurrentQueue<byte[]> _inbox;
        /// <summary>
        /// Indicates object disposal state.
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Open a serial COM port with the given number.
        /// </summary>
        /// <param name="comPortNumber">COM port number.</param>
        internal SerialPortAccessLayer(int comPortNumber)
        {
            _serialPort = new SerialPort("COM" + comPortNumber.ToString());
            _rxBuffer = new ConcurrentQueue<byte>();
            _inbox = new ConcurrentQueue<byte[]>();
            _disposed = false;
            _serialPort.DataReceived += async (object sender, SerialDataReceivedEventArgs args) =>
            {
                SerialPort serialPort = sender as SerialPort;
                byte[] readBuffer = new byte[serialPort.BytesToRead];

                await serialPort.BaseStream.ReadAsync(readBuffer, 0, readBuffer.Length);
                
                // Loop buffering incoming bytes until all bytes read
                foreach (byte currentByte in readBuffer)
                    _rxBuffer.Enqueue(currentByte);

                // Loop buffering byte packets until there's fewer bytes than a whole packet
                while (_rxBuffer.Count >= PACKET_SIZE)
                {
                    List<byte> currentPacket = new List<byte>();

                    // Loop populating the current packet until it's full
                    for (int n = 1; n <= PACKET_SIZE; n++)
                    {
                        byte currentByte;

                        // Loop polling for the next buffered byte until it's available
                        while (!_rxBuffer.TryDequeue(out currentByte))
                            await Task.Delay(POLLING_DURATION);

                        currentPacket.Add(currentByte);
                    }

                    _inbox.Enqueue(currentPacket.ToArray());
                }
            };

            _serialPort.Open();
        }

        /// <summary>
        /// Transmit the given packet of byte value.
        /// </summary>
        /// <param name="packet">Byte packet.</param>
        internal async Task transmitAsync(params byte[] packet)
        {
            // Abort if the packet is not the correct size
            if (packet.Length != PACKET_SIZE)
                throw new ArgumentException($"The given packet must be {PACKET_SIZE} bytes long.");

            await _serialPort.BaseStream.WriteAsync(packet, 0, packet.Length);
        }

        /// <summary>
        /// Receive a packet of byte values.
        /// </summary>
        /// <returns>Byte packet.</returns>
        internal async Task<byte[]> receiveAsync()
        {
            byte[] currentPacket = null;

            // Loop polling for the next buffered packet until it's available
            while (!_inbox.TryDequeue(out currentPacket))
                await Task.Delay(POLLING_DURATION);

            return currentPacket;
        }

        ~SerialPortAccessLayer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            _serialPort.Dispose();

            _disposed = true;
        }
    }
}