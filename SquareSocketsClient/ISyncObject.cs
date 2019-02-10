using System;
using System.Collections.Generic;
using System.Text;

namespace SquareSocketsClient {
    internal interface ISyncObject {
        Dictionary<string, string> Variables { get; set; }

        void CreateVariables();
    }
}
