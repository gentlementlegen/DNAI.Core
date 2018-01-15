using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command
{
    public class SetInstructionInputValue : ICommand<SetInstructionInputValue.Reply>
    {
        public class Reply
        {
            [BinarySerializer.BinaryFormat]
            public SetInstructionInputValue Command { get; set; }
        }

        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputValue { get; set; }

        public Reply Resolve(Controller controller)
        {
            controller.SetInstructionInputValue(FunctionID, Instruction, InputName, JsonConvert.DeserializeObject(InputValue));
            return new Reply
            {
                Command = this
            };
        }
    }
}