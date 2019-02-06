using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SquareSocketsClient {
    internal class Messenger {
        private Socket Socket { get; set; } // 

        public Messenger(Socket socket) {
            Socket = socket;
        }

        public void Start() {
            Thread sendInput = new Thread(ReadInput);
            sendInput.Start();
        }

        private void ReadInput() {
            while (true) {
                string consoleInput = Console.ReadLine();
                Send(consoleInput);
            }
        }

        private void Send(string data) {
            try {
                List<byte> fullPacket = new List<byte>();
                fullPacket.AddRange(BitConverter.GetBytes(data.Length));
                fullPacket.AddRange(Encoding.Default.GetBytes(data));
                Socket.Send(fullPacket.ToArray());
            } catch (Exception ex) {
                Console.WriteLine("Error " + ex);
            }
        }
    }
}
