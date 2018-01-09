using System;

namespace CoreCommand.Command
{
    public class GetVariableValue
    {
        [BinarySerializer.BinaryFormat]
        public UInt32 VariableId { get; set; }
    }
}