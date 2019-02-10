using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SquareSocketsClient {
    internal class Receiver {
        private Socket Socket { get; set; }

        private byte[] buffer;

        public Receiver(Socket socket) {
            Socket = socket;
        }

        public void Start() {
            Receive();
        }

        private void Receive() {
            try {
                buffer = new byte[4];
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            } catch {

            }
        }

        private void ReceiveCallback(IAsyncResult AR) {
            try {
                if (Socket.EndReceive(AR) > 1) {
                    buffer = new byte[BitConverter.ToInt32(buffer, 0)];
                    Socket.Receive(buffer, buffer.Length, SocketFlags.None);

                    string data = Encoding.UTF8.GetString(buffer);
                    Console.WriteLine(data);

                    Receive();
                } else {
                    Disconnect();
                }
            } catch {
                if (!Socket.Connected) {
                    Disconnect();
                } else {
                    Receive();
                }
            }
        }

        private void Disconnect() {
            Socket.Disconnect(true);
        }
    }
}
