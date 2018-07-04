using CoreControl;
using Newtonsoft.Json;

namespace CoreCommand.Command.Function.Instruction
{
    public class SetInputValue : ICommand<EmptyReply>
    {
        [BinarySerializer.BinaryFormat]
        public uint FunctionID { get; set; }

        [BinarySerializer.BinaryFormat]
        public uint Instruction { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputName { get; set; }

        [BinarySerializer.BinaryFormat]
        public string InputValue { get; set; }

        public EmptyReply Resolve(Controller controller)
        {
            controller.SetInstructionInputValue(FunctionID, Instruction, InputName, JsonConvert.DeserializeObject(InputValue));
            return null;
        }
    }
}