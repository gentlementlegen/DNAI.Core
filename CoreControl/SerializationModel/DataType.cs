using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreControl.SerializationModel
{
    public class DataType
    {
        public enum WHICH
        {
            BOOLEAN,
            INTEGER,
            FLOATING,
            CHARACTER,
            STRING,
            DICT,
            ANY
        }

        [BinarySerializer.BinaryFormat]
        public WHICH TypeID { get; set; }

    }
}
