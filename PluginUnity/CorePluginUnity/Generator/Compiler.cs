using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;
[assembly: InternalsVisibleTo("TestUnityPlugin")]

namespace Core.Plugin.Unity.Generator
{
    /// <summary>
    /// Compiler class that will allow C# code to be compiled at runtime.
    /// Creates a new library according to the supplied source code, and
    /// does not create any temporary code file.
    /// </summary>
    internal class Compiler
    {
        private readonly CSharpCodeProvider _provider = new CSharpCodeProvider();
        private readonly CompilerParameters _parameters = new CompilerParameters();

        private readonly string code = @"
    using System;

    namespace First
    {
        public class Program
        {
            public static void Main()
            {
            " +
                "Console.WriteLine(\"Hello, world!\");"
                + @"
            }
        }
    }
";

        internal Compiler()
        {
            // Reference to library
            _parameters.ReferencedAssemblies.Add("UnityEngine.dll");
            // True - memory generation, false - external file generation
            _parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            _parameters.GenerateExecutable = false;
            _parameters.OutputAssembly = "DulyGeneratedAssembly.dll";
        }

        /// <summary>
        /// Compiles the code to an assembly. <para/>
        /// Throws <see cref="InvalidOperationException"/> on compilation failure.
        /// </summary>
        internal CompilerResults Compile()
        {
            CompilerResults results = _provider.CompileAssemblyFromSource(_parameters, code);

            if (results.Errors.HasErrors)
            {
                var sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendFormat("Error ({0}): {1}", error.ErrorNumber, error.ErrorText).AppendLine();
                }

                throw new InvalidOperationException(sb.ToString());
            }
            return results;
        }
    }
}