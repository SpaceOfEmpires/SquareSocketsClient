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
            try {
                buffer = new byte[4];
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            } catch {

            }
        }

        private void ReceiveCallback(IAsyncResult AR) {
            try {
                // if bytes are less than 1 takes place when a client disconnect from the server.
                // So we run the Disconnect function on the current client
                if (Socket.EndReceive(AR) > 1) {
                    // Convert the first 4 bytes (int 32) that we received and convert it to an Int32 (this is the size for the coming data).
                    buffer = new byte[BitConverter.ToInt32(buffer, 0)];
                    // Next receive this data into the buffer with size that we did receive before
                    Socket.Receive(buffer, buffer.Length, SocketFlags.None);
                    // When we received everything its onto you to convert it into the data that you've send.
                    // For example string, int etc... in this example I only use the implementation for sending and receiving a string.

                    // Convert the bytes to string and output it in a message box
                    string data = Encoding.Default.GetString(buffer);
                    Console.WriteLine(data);
                    // Now we have to start all over again with waiting for a data to come from the socket.
                    Start();
                } else {
                    Disconnect();
                }
            } catch {
                // if exeption is throw check if socket is connected because than you can startreive again else Dissconect
                if (!Socket.Connected) {
                    Disconnect();
                } else {
                    Start();
                }
            }
        }

        private void Disconnect() {
            Socket.Disconnect(true);
        }
    }
}
