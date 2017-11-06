using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Error
{
    /// <summary>
    /// Exception to specify that something hasn't been found
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception innerException) : base(message, innerException) { }

        public NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
