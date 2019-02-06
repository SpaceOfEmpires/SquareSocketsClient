using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Diagnostics;

namespace SquareSocketsClient {
    class Program {
        private static ManualResetEvent endMainThread = new ManualResetEvent(false);

        static void Main(string[] args) {
            Console.WriteLine("Client");
            Client client = new Client();
            client.Start();
            endMainThread.WaitOne();
        }
    }
}