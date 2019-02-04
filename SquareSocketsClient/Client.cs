using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Diagnostics;

namespace SquareSocketsClient {
    internal class Client {
        public Socket _socket { get; set; }
        public int _id { get; set; }

        public ReceivePacket Receive { get; set; }
        public SendPacket Send { get; set; }

        public Client() {
            _id = 1;
        }

        public void SetClient(Socket socket) {
            _socket = socket;
            Receive = new ReceivePacket(socket);
            Send = new SendPacket(socket);
        }

        public void SendString(string msg) {
            Send.Send(msg);
        }


        /*private const int port = 11000; // The port number for the remote device

        // ManualResetEvent instances signal completion
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);

        private ManualResetEvent endMainThread = new ManualResetEvent(false); // Keep the main thread from stopping

        private String response = String.Empty; // The response from the remote device

        public void StartClient() {
            // Connect to a remote device
            try {
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // Dns.GetHostEntry("host.contoso.com");
                IPAddress ipAddress = null;

                // Find and use the first ipv4 address
                for (int i = 0; i < ipHostInfo.AddressList.Length; i++) {
                    if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork) {
                        ipAddress = ipHostInfo.AddressList[i];
                    }
                }

                // Stop in case no ip is set
                if (ipAddress == null) {
                    Debug.WriteLine("ipAddress null");
                    return;
                } else {
                    Debug.WriteLine("Will be connecting to: " + ipAddress);
                }

                IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket

                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client); // Connect to the remote endpoint
                connectDone.WaitOne();

                Thread sendInput = new Thread(SendInput);
                sendInput.Start(client);
                Thread listen = new Thread(Listen);
                listen.Start(client);

                // Release the socket
                //client.Shutdown(SocketShutdown.Both);
                //client.Close();

                endMainThread.WaitOne();
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }



        private void SendInput(object client) {
            while (true) {
                string consoleInput = Console.ReadLine();
                consoleInput += "<EOF>";

                Debug.WriteLine(consoleInput);

                // Send test data to the remote device
                Send((Socket)client, consoleInput);
                sendDone.WaitOne();
            }
        }

        private void Listen(object client) {
            while (true) {
                // Receive the response from the remote device
                Receive((Socket)client);
                receiveDone.WaitOne();

                Console.WriteLine("Response received : {0}", response); // Write the response to the console
            }
        }






        private void ConnectCallback(IAsyncResult ar) {
            try {
                Socket client = (Socket)ar.AsyncState; // Retrieve the socket from the state object
                client.EndConnect(ar); // Complete the connection
                Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                connectDone.Set(); // Signal that the connection has been made
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void Receive(Socket client) {
            try {
                // Create the state object
                StateObject state = new StateObject();
                state.workSocket = client;
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state); // Begin receiving the data from the remote device
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void ReceiveCallback(IAsyncResult ar) {
            try {
                // Retrieve the state object and the client socket from the asynchronous state object
                StateObject state = (StateObject)ar.AsyncState;
                Socket client = state.workSocket;

                // Read data from the remote device
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0) {
                    // There might be more data, so store the data received so far
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                    // Get the rest of the data
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                        new AsyncCallback(ReceiveCallback), state);
                } else {
                    // All the data has arrived; put it in response
                    if (state.sb.Length > 1) {
                        response = state.sb.ToString();
                    }
                    // Signal that all bytes have been received
                    receiveDone.Set();
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }

        private void Send(Socket client, String data) {
            byte[] byteData = Encoding.ASCII.GetBytes(data); // Convert the string data to byte data using ASCII encoding

            client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client); // Begin sending the data to the remote device
        }

        private void SendCallback(IAsyncResult ar) {
            try {
                Socket client = (Socket)ar.AsyncState; // Retrieve the socket from the state object

                // Complete sending the data to the remote device
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                sendDone.Set(); // Signal that all bytes have been sent
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
            }
        }*/
    }
}
