using System.Net.Http;
using System.Threading;
using System;
using System.Net.Sockets;

namespace ClientClassNameSpace
{
    public class ClientClass
    {
        private readonly string _serverAddress;
        private readonly int _port;
        private NetworkStream _stream;
        private Thread _listeningThread;
        private bool _isListening;
        private TcpClient _client;

        public ClientClass(string serverAddres, int port)
        {
            _serverAddress = serverAddres;
            _port = port;
        }

        public ClientClass(TcpClient client)
        {
            _client = client;
        }

//Можно попробывать создать отдельный метод, который в качестве параметра принимает client
        public void Connect()
        {
            _client = _client ?? new TcpClient(_serverAddress, _port);
            _stream = _client.GetStream();
            StartListening();
        }

        public void SendMessages(string message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
            _stream.Write(data, 0, data.Length);
        }

        public event Action<string> OnMessageRecieved;

        private void StartListening()
        {
            _isListening = true;

            _listeningThread = new Thread(() =>
            {
                while (_isListening)
                {
                    byte[] data = new byte[256];
                    Int32 bytes = _stream.Read(data, 0, data.Length);
                    string message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    OnMessageRecieved?.Invoke(message);
                }
            });

            _listeningThread.Start();
        }

        private void StopListening()
        {
            _isListening = false;
        }

        public void Disconnect()
        {
            StopListening();
            _stream.Close();
            _client.Close();
        }
    }
}