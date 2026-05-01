using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace OmniCore.SDK
{
    // ============================================================================
    // OMNI CORE - C# / UNITY CLIENT SDK
    // HIGH-FREQUENCY ZERO-ALLOCATION NETWORK BRIDGE
    // ============================================================================
    public class OmniClient : IDisposable
    {
        private readonly Socket _socket;
        private readonly EndPoint _targetEndPoint;

        // Pre-allocated memory buffer to ensure Zero-GC (Garbage Collection) operation
        private readonly byte[] _hftBuffer;
        private readonly object _lockObj = new object();

        /// <summary>
        /// Initializes a new instance of the OmniClient for high-performance UDP transmission.
        /// </summary>
        /// <param name="ipAddress">The target IPv4 address.</param>
        /// <param name="port">The target port (Default: 9999).</param>
        /// <param name="maxPayloadSize">Maximum expected payload size in bytes.</param>
        public OmniClient(string ipAddress, int port = 9999, int maxPayloadSize = 2048)
        {
            _targetEndPoint = new IPEndPoint(IPAddress.Parse(ipAddress), port);

            // Utilizing raw Socket instead of UdpClient for maximum OS-level control
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);

            // One-time allocation of the network buffer
            _hftBuffer = new byte[1 + maxPayloadSize];
        }

        // ============================================================================
        // 1. HFT (HIGH-FREQUENCY TRADING) API - ZERO GC ALLOCATION
        // ============================================================================

        /// <summary>
        /// Hot-path execution method for high-frequency loops (e.g., Unity Update()).
        /// Operates without 'new' allocations or 'async/await' state machines.
        /// Fires immediately at the OS level (non-blocking).
        /// </summary>
        /// <param name="opCode">The routing identifier (0x00 - 0xFF).</param>
        /// <param name="payload">The byte array containing the data.</param>
        /// <param name="length">The exact length of the data to send.</param>
        public void SendHFT(byte opCode, byte[] payload, int length)
        {
            if (length > _hftBuffer.Length - 1) return; // Safety boundary check

            // Thread safety for concurrent multi-threaded execution
            lock (_lockObj)
            {
                _hftBuffer[0] = opCode;
                Buffer.BlockCopy(payload, 0, _hftBuffer, 1, length);

                // Direct kernel call, zero heap allocation
                _socket.SendTo(_hftBuffer, 0, length + 1, SocketFlags.None, _targetEndPoint);
            }
        }

        // ============================================================================
        // 2. STANDARD API - CONVENIENCE WRAPPERS
        // ============================================================================

        /// <summary>
        /// Standard send method for general byte array payloads.
        /// </summary>
        public void Send(byte opCode, byte[] payload)
        {
            SendHFT(opCode, payload, payload.Length);
        }

        /// <summary>
        /// Convenience method for string messages.
        /// WARNING: Encoding.UTF8.GetBytes() allocates memory on the managed heap (GC Spike).
        /// Use only for infrequent events (e.g., chat messages, login sequences).
        /// </summary>
        public void SendString(byte opCode, string message)
        {
            byte[] payload = Encoding.UTF8.GetBytes(message);
            SendHFT(opCode, payload, payload.Length);
        }

        // ============================================================================
        // RESOURCE MANAGEMENT
        // ============================================================================

        /// <summary>
        /// Safely releases the network socket and associated resources.
        /// </summary>
        public void Dispose()
        {
            if (_socket != null)
            {
                _socket.Close();
                _socket.Dispose();
            }
        }
    }
}