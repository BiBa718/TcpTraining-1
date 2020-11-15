

using System;
using System.Net.Sockets;

namespace ClientClassNameSpace
{
    public class ClientClass
    {
        private readonly string _serverAddress;
        private readonly int _port;

        public ClientClass(string serverAddres, int port)
        {
            _serverAddress = serverAddres;
            _port = port;
        }

        public void Connect()
        {
            TcpClient client = new TcpClient(_serverAddress, _port);
            NetworkStream stream = client.GetStream();
        }

    
    }
}