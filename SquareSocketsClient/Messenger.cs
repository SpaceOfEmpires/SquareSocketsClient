using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

namespace SquareSocketsClient {
    internal class Messenger {
        private Socket Socket { get; set; } // 

        private List<ISyncObject> ObjectsToSync { get; set; }

        public Messenger(Socket socket) {
            Socket = socket;

            ObjectsToSync = new List<ISyncObject>();
            ObjectsToSync.Add(new TestObject1("obj1"));
            ObjectsToSync.Add(new TestObject1("obj2"));
        }

        public void Start() {
            Thread readInput = new Thread(ReadInput);
            readInput.Start();
        }

        private void ReadInput() {
            while (true) {
                string consoleInput = Console.ReadLine();

                switch (consoleInput) {
                    case "a":
                        foreach (ISyncObject obj in ObjectsToSync) {
                            obj.CreateVariables();
                        }

                        Send(JsonConvert.SerializeObject(ObjectsToSync));
                        break;
                    case "b":
                        foreach (ISyncObject obj in ObjectsToSync) {
                            (obj as TestObject1).RandomizeAge();
                        }
                        break;
                    default:
                        Send(consoleInput);
                        break;
                }
            }
        }

        private void Send(string data) {
            try {
                List<byte> fullPacket = new List<byte>();
                byte[] package = Encoding.UTF8.GetBytes(data);
                fullPacket.AddRange(BitConverter.GetBytes(package.Length));
                fullPacket.AddRange(package);
                Socket.Send(fullPacket.ToArray());
            } catch (Exception ex) {
                Console.WriteLine("Error " + ex);
            }
        }
    }
}
