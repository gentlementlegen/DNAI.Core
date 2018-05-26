using System;
using System.Collections.Generic;
using CoreControl;
using static CoreControl.InstructionFactory;

namespace CoreCommand.Command.Function
{
    public class AddInstruction : ICommand<AddInstruction.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public UInt32 InstructionID { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public UInt32 FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public INSTRUCTION_ID ToCreate { get; set; }

        [BinarySerializer.BinaryFormat]
        public List<UInt32> Arguments { get; set; } = new List<UInt32>();

        public Reply Resolve(Controller controller)
        {
            return new Reply
            {
                InstructionID = controller.AddInstruction(FunctionID, ToCreate, Arguments)
            };
        }
    }
}