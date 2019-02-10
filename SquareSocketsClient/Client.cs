using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System.Diagnostics;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SquareSocketsClient {
    internal class Client {
        private Socket Socket { get; set; } // The socket which will connect to the server

        private IPAddress ip; // Ip address to connect to
        private readonly short port = 11000; // Port to connect to

        private byte[] buffer; // Byte array used in figuring out when the incomming package ends

        private List<ISyncObject> ObjectsToSync { get; set; } // List of objects received from the server

        public Client() {
            ip = GetIpAddress(); //IPAddress.Parse("10.0.0.4");
            ObjectsToSync = new List<ISyncObject>(); // Initialize the objects list
        }

        /// <summary>
        /// Setup the connection and start listening for incomming packages
        /// </summary>
        public void Start() {
            Connect(); // Connect to the server. Will only continue from here when a connection is established
            Receive(); // Start listening for incomming packages from the server

            // This is just for testing purposes. This will not work in unity (unless theres a console that opens up?)
            Thread readInput = new Thread(ReadInput);
            readInput.Start();
        }

        private void Connect() {
            bool failed = false;
            Socket = new Socket(ip.AddressFamily, SocketType.Stream, ProtocolType.Tcp); // Initialize a new socket that will use an ipv4 address and tcp. Socket types: https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.sockettype?view=netframework-4.7.2
            // If a connection is not established then keep trying
            while (!Socket.Connected) {
                try {
                    Socket.Connect(new IPEndPoint(ip, port)); // Connect to an endpoint of the ip and port
                    Console.WriteLine("Connected");
                } catch {
                    if (!failed) {
                        failed = true;
                        Console.WriteLine("Failed to connect to server");
                    }
                    Thread.Sleep(1000); // Wait a second before trying to connect again
                }
            }
        }

        /// <summary>
        /// Begin listening for incomming packages
        /// </summary>
        private void Receive() {
            try {
                buffer = new byte[4]; // Set the buffer to a size of 4 (int32)
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null); // Listen for a package. Receive 4 bytes (buffer). Then run the AsyncCallback to handle the incomming package
            } catch(Exception ex) {
                Console.WriteLine("Failed to receive package from server!");
                Console.WriteLine("Error: " + ex);
            }
        }

        /// <summary>
        /// AsyncCallback that is run when a package has arrived
        /// </summary>
        private void ReceiveCallback(IAsyncResult AR) {
            try {
                if (Socket.EndReceive(AR) > 1) {
                    buffer = new byte[BitConverter.ToInt32(buffer, 0)]; // Get the 4 first bytes from the incomming package and turn the value into a new byte array. The new byte array is the length of the rest of the incomming package
                    Socket.Receive(buffer, buffer.Length, SocketFlags.None); // Receive the last of the package



                    string data = Encoding.UTF8.GetString(buffer); // Turn the package of bytes into a string using the encoding UTF8
                    Console.WriteLine(data); // The message should rather be send to a handler



                    Receive(); // Start listening for new incomming packages
                } else {
                    Disconnect(); // If we didnt receive more than one byte then the server disconnected
                }
            } catch(ObjectDisposedException ex) {
                
            } catch(Exception ex) {
                Console.WriteLine(ex);
                if (!Socket.Connected) {
                    Disconnect(); // Disconnect this socket if the server disconnected
                } else {
                    Receive(); // If the server didnt disconnect then try listening for a new package
                }
            }
        }

        /// <summary>
        /// Disconnect the socket
        /// </summary>
        private void Disconnect() {
            try {
                Socket.Shutdown(SocketShutdown.Both);
                Socket.Close();
                Console.WriteLine("disconnected");
            } catch (Exception ex) {
                Console.WriteLine("Error: " + ex);
            }
        }

        /// <summary>
        /// Take a string and encode it to finally send it over the socket
        /// </summary>
        public void Send(string data) {
            try {
                List<byte> fullPacket = new List<byte>(); // Using a list to easier add onto the array
                byte[] package = Encoding.UTF8.GetBytes(data); // Get the string as a byte array
                fullPacket.AddRange(BitConverter.GetBytes(package.Length)); // Get the length of the package in bytes and add it to the list
                fullPacket.AddRange(package); // Now add the actual package to the list
                Socket.Send(fullPacket.ToArray()); // Send the list to the socket as an array
            } catch (Exception ex) {
                Console.WriteLine("Error " + ex);
            }
        }

        // TODO: This should do a dns lookup on a domain name and not this machine. Unless we are connecting locally, but that can wait
        /// <summary>
        /// Get the first ipv4 address of this machine
        /// </summary>
        private IPAddress GetIpAddress() {
            IPAddress ip = null;
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName()); // Perform a dns lookup on this machine to get all the ip addresses
            // Find and use the first ipv4 address from the dns lookup
            for (int i = 0; i < ipHostInfo.AddressList.Length; i++) {
                if (ipHostInfo.AddressList[i].AddressFamily == AddressFamily.InterNetwork) {
                    ip = ipHostInfo.AddressList[i];
                }
            }
            if (ip == null) {
                Console.WriteLine("Server ip address not found");
            } else {
                Console.WriteLine("Server ip address found: " + ip);
                Console.WriteLine("Server port to connect to: " + port);
            }
            return ip;
        }

        /// <summary>
        /// Reads the input from the console and performs an action depending on the command
        /// </summary>
        private void ReadInput() {
            while (true) {
                string consoleInput = Console.ReadLine();

                if (consoleInput.StartsWith("/")) {
                    consoleInput = consoleInput.Remove(0, 1);
                    switch (consoleInput) {
                        case "connect":
                            if (!Socket.Connected) { Connect(); } else { Console.WriteLine("Already connected to server!"); }
                            break;
                        case "disconnect":
                            if (Socket.Connected) { Disconnect(); } else { Console.WriteLine("Already disconnected from server!"); }
                            break;
                        default:
                            break;
                    }
                } else {
                    if (Socket.Connected) {
                        switch (consoleInput) {
                            case "a":


                                /*foreach (ISyncObject obj in ObjectsToSync) {
                                    obj.CreateVariables();
                                }
                                Send(JsonConvert.SerializeObject(ObjectsToSync));*/
                                break;
                            /*case "b":
                                foreach (ISyncObject obj in ObjectsToSync) {
                                    (obj as TestObject1).RandomizeAge();
                                }
                                break;*/
                            default:
                                Send(consoleInput);
                                break;
                        }
                    } else {
                        Console.WriteLine("No connection to the server");
                    }
                }
            }
        }

        private void RequestObject() {

        }
    }
}
