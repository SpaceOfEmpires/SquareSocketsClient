using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Diagnostics;

namespace SquareSocketsClient {
    internal class Client {
        private Socket Socket { get; set; } // The socket which will connect to the server

        private Receiver Receiver { get; set; } // 
        private Messenger Messenger { get; set; } // 

        private IPAddress ip; // Ip address to listen on
        private readonly short port = 11000; // Port to listen on

        public Client() {
            ip = GetIpAddress();
        }

        public void Start() {
            Socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            while (!Socket.Connected) {
                Thread.Sleep(1000);
                try {
                    Socket.Connect(new IPEndPoint(ip, port));
                } catch {

                }
            }
            Receiver = new Receiver(Socket);
            Receiver.Start();
            Messenger = new Messenger(Socket);
            Messenger.Start();
        }

        private IPAddress GetIpAddress() {
            IPAddress ip = null;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            // Find and use the first ipv4 address of this pc
            for (int i = 0; i < ipHostInfo.AddressList.Length; i++) {
                if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork) {
                    ip = ipHostInfo.AddressList[i];
                }
            }
            if (ip == null) { Console.WriteLine("Server ip address not found"); } else {
                Console.WriteLine("Server ip address found: " + ip);
                Console.WriteLine("Server port to listen on: " + port);
            }

            return ip;
        }
    }
}
