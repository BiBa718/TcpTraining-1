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

        public ClientClass(string serverAddres, int port)
        {
            _serverAddress = serverAddres;
            _port = port;
        }

        public void Connect()
        {
            TcpClient client = new TcpClient(_serverAddress, _port);
            _stream = client.GetStream();
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
            _listeningThread = new Thread(() =>
            {
                //todo fix infinity loop
                while (true)
                {
                    byte[] data = new byte[256];
                    Int32 bytes = _stream.Read(data, 0, data.Length);
                    string message = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    OnMessageRecieved?.Invoke(message);
                }
            });

            _listeningThread.Start();
        }
    }
}