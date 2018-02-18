using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Error
{
    class OperatorNotOverloaded : Exception
    {
        public OperatorNotOverloaded() : base("This operator haven't be overloaded yet") { }

        public OperatorNotOverloaded(string message) : base(message) { }

        public OperatorNotOverloaded(string message, Exception exception) : base(message, exception) { }

        public OperatorNotOverloaded(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
