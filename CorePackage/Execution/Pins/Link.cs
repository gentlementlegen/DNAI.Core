using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CorePackage.Execution
{
    public class Link
    {
        private Execution.Instruction instruction;

        private string output;

        public Link(Instruction tolink, string outputname)
        {
            instruction = tolink;
            output = outputname;
        }

        public dynamic Value
        {
            get { return instruction.GetOutput(output).Value; }
        }

        public Instruction Instruction
        {
            get { return instruction; }
        }

        public string Output
        {
            get { return output; }
        }
    }
}
