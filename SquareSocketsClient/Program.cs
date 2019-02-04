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
            Connector connector = new Connector();
            connector.Start();
            endMainThread.WaitOne();
        }

        /*class Program {
            private const int port = 11000; // The port number for the remote device

            // ManualResetEvent instances signal completion
            private static ManualResetEvent connectDone = new ManualResetEvent(false);
            private static ManualResetEvent sendDone = new ManualResetEvent(false);
            private static ManualResetEvent receiveDone = new ManualResetEvent(false);

            private static String response = String.Empty; // The response from the remote device

            static void Main(string[] args) {
                Console.WriteLine("Client");
                StartClient();
            }

            private static void StartClient() {
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
                        Debug.WriteLine("Listening on ip: " + ipAddress);
                    }

                    IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

                    Socket client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Create a TCP/IP socket

                    client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client); // Connect to the remote endpoint
                    connectDone.WaitOne();


                    string consoleInput = Console.ReadLine();
                    consoleInput += "<EOF>";

                    // Send test data to the remote device
                    Send(client, consoleInput);
                    sendDone.WaitOne();

                    // Receive the response from the remote device
                    Receive(client);
                    receiveDone.WaitOne();

                    Console.WriteLine("Response received : {0}", response); // Write the response to the console

                    // Release the socket
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();

                    Console.Read();
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void ConnectCallback(IAsyncResult ar) {
                try {
                    Socket client = (Socket)ar.AsyncState; // Retrieve the socket from the state object
                    client.EndConnect(ar); // Complete the connection
                    Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                    connectDone.Set(); // Signal that the connection has been made
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void Receive(Socket client) {
                try {
                    // Create the state object
                    StateObject state = new StateObject();
                    state.workSocket = client;
                    client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state); // Begin receiving the data from the remote device
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }

            private static void ReceiveCallback(IAsyncResult ar) {
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

            private static void Send(Socket client, String data) {
                byte[] byteData = Encoding.ASCII.GetBytes(data); // Convert the string data to byte data using ASCII encoding

                client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), client); // Begin sending the data to the remote device
            }

            private static void SendCallback(IAsyncResult ar) {
                try {
                    Socket client = (Socket)ar.AsyncState; // Retrieve the socket from the state object

                    // Complete sending the data to the remote device
                    int bytesSent = client.EndSend(ar);
                    Console.WriteLine("Sent {0} bytes to server.", bytesSent);

                    sendDone.Set(); // Signal that all bytes have been sent
                } catch (Exception e) {
                    Console.WriteLine(e.ToString());
                }
            }
        }*/
    }
}