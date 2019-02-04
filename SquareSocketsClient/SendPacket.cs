using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace SquareSocketsClient {
    class SendPacket {
        private Socket _sendSocket;

        public SendPacket(Socket sendSocket) {
            _sendSocket = sendSocket;
        }

        public void Send(string data) {
            try {
                List<byte> fullPacket = new List<byte>();
                fullPacket.AddRange(BitConverter.GetBytes(data.Length));
                fullPacket.AddRange(Encoding.Default.GetBytes(data));
                _sendSocket.Send(fullPacket.ToArray());
            } catch (Exception ex) {
                throw ex;
            }
        }
    }
}
