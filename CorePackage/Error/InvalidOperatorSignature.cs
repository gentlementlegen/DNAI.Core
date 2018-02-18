using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Error
{
    class InvalidOperatorSignature : Exception
    {
        public InvalidOperatorSignature() : base() { }

        public InvalidOperatorSignature(string message) : base(message) { }

        public InvalidOperatorSignature(string message, Exception exception) : base(message, exception) { }

        public InvalidOperatorSignature(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
