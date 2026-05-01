using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

namespace OmniCore.SDK
{
    [DefaultExecutionOrder(-100)]
    public class OmniCoreClient : MonoBehaviour
    {
        [Header("Omni Core Server Configuration")]
        [Tooltip("The IP address of the Omni Core server")]
        public string serverIP = "127.0.0.1";

        [Tooltip("The target port (although AF_PACKET intercepts everything, the router requires it)")]
        public int serverPort = 9000;

        [Tooltip("The OpCode of the target module (e.g., 0x01 for Diagnostic Tentacle)")]
        public byte targetOpCode = 0x01;

        private UdpClient _udpClient;
        private IPEndPoint _endPoint;
        private byte[] _packetBuffer;

        void Start()
        {
            InitializeCoreConnection();
        }

        private void InitializeCoreConnection()
        {
            try
            {
                _udpClient = new UdpClient();
                _endPoint = new IPEndPoint(IPAddress.Parse(serverIP), serverPort);

                // Pre-allocated buffer to bypass the Garbage Collector (GC)
                _packetBuffer = new byte[2048];

                Debug.Log($"[Omni Core] Client initialized. Target: {serverIP}:{serverPort}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Omni Core] Initialization error: {ex.Message}");
            }
        }

        /// <summary>
        /// Sends a raw byte array to the Omni Core with automatic OpCode injection.
        /// </summary>
        /// <param name="payload">The data to be sent</param>
        /// <param name="payloadLength">The length of the data in bytes</param>
        public void SendPayload(byte[] payload, int payloadLength)
        {
            if (_udpClient == null || payload == null || payloadLength == 0) return;

            // Security check to prevent buffer overflow
            if (payloadLength + 1 > _packetBuffer.Length)
            {
                Debug.LogWarning("[Omni Core] The payload exceeds the maximum buffer size (2048 bytes).");
                return;
            }

            // [Byte 0] = OpCode
            _packetBuffer[0] = targetOpCode;

            // [Bytes 1..N] = Fast payload copy
            Buffer.BlockCopy(payload, 0, _packetBuffer, 1, payloadLength);

            try
            {
                _udpClient.Send(_packetBuffer, payloadLength + 1, _endPoint);
            }
            catch (SocketException ex)
            {
                Debug.LogError($"[Omni Core] Network error during transmission: {ex.Message}");
            }
        }

        void OnDestroy()
        {
            if (_udpClient != null)
            {
                _udpClient.Close();
                _udpClient.Dispose();
            }
        }
    }
}