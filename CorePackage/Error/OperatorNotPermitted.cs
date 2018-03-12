using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Error
{
    /// <summary>
    /// Exception to specify that an Operator is not permitted on a specific type
    /// </summary>
    class OperatorNotPermitted : Exception
    {
        public OperatorNotPermitted() : base() { }

        public OperatorNotPermitted(string message) : base(message) { }

        public OperatorNotPermitted(string message, Exception exception) : base(message, exception) { }

        public OperatorNotPermitted(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
