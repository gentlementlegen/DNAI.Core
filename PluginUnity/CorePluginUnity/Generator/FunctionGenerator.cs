using System.Collections.Generic;

namespace Core.Plugin.Unity.Generator
{
    public partial class GenerateFunctionTemplate
    {
        public List<string> Inputs = new List<string>();
        public List<string> Outputs = new List<string>();
        public uint FunctionId;
        public string FunctionArguments = "";
    }
}