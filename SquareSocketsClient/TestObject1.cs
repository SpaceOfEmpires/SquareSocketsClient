using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SquareSocketsClient {
    internal class TestObject1 : ISyncObject {
        public Dictionary<string, string> Variables { get; set; }

        private string Name { get; set; }
        private int Age { get; set; }

        Random rnd = new Random();

        public TestObject1(string name) {
            Name = name;
        }

        public void CreateVariables() {
            Variables = new Dictionary<string, string>();

            Variables.Add("this", this.GetType().Name);
            Variables.Add(nameof(Name), JsonConvert.SerializeObject(Name));
            Variables.Add(nameof(Age), JsonConvert.SerializeObject(Age));
        }

        public void RandomizeAge() {
            Age = rnd.Next(0, 100);
        }
    }
}
