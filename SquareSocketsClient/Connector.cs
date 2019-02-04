using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SquareSocketsClient {
    class Connector {
        private Socket _connectingSocket;
        public short port = 11000; // Port to listen on

        Client client;

        public Connector() {
            client = new Client();
        }

        public void Start() {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = null;
            // Find and use the first ipv4 address of this pc
            for (int i = 0; i < ipHostInfo.AddressList.Length; i++) {
                if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork) {
                    ipAddress = ipHostInfo.AddressList[i];
                }
            }
            // Stop in case no ip have been found
            if (ipAddress == null) {
                Debug.WriteLine("Client ip address not found");
                return;
            } else {
                Debug.WriteLine($"Client connecting to: {ipAddress}:{port}");
            }

            _connectingSocket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            while (!_connectingSocket.Connected) {
                Thread.Sleep(1000);

                try {
                    _connectingSocket.Connect(new IPEndPoint(ipAddress, port));
                } catch { }
            }
            SetupForReceiveing();
        }

        private void SetupForReceiveing() {
            client.SetClient(_connectingSocket);
            client.Receive.StartReceiving();
            Thread sendInput = new Thread(SendInput);
            sendInput.Start(client);
        }

        private void SendInput(object client1) {
            while (true) {
                string consoleInput = Console.ReadLine();

                Debug.WriteLine(consoleInput);

                client.SendString(consoleInput);

                // Send test data to the remote device
                //Send((Socket)client, consoleInput);
                //sendDone.WaitOne();
            }
        }
    }
}
