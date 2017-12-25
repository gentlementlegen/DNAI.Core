using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Text;
using System.Runtime.CompilerServices;
using System.IO;

[assembly: InternalsVisibleTo("TestUnityPlugin")]

namespace Core.Plugin.Unity.Generator
{
    /// <summary>
    /// Manages code conversion from Duly files to Unity files.
    /// </summary>
    public class DulyCodeConveter
    {
        private readonly Compiler _compiler = new Compiler();
        private readonly TemplateReader _template = new TemplateReader();

        /// <summary>
        /// Transform Duly code to Unity code.
        /// </summary>
        public void ConvertCode()
        {
            var code = _template.GenerateTemplateContent();
            _compiler.Compile(code);
        }
    }

    /// <summary>
    /// Compiler class that will allow C# code to be compiled at runtime.
    /// Creates a new library according to the supplied source code, and
    /// does not create any temporary code file.
    /// </summary>
    internal class Compiler
    {
        private readonly CSharpCodeProvider _provider = new CSharpCodeProvider();
        private readonly CompilerParameters _parameters = new CompilerParameters();

        private string code = @"
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

        internal CompilerResults Compile(string code, string outputPath = "./")
        {
            this.code = code;
            return Compile();
        }

        internal Compiler()
        {
            // Reference to library
            // TODO : change with instllation program
            _parameters.ReferencedAssemblies.Add(Environment.ExpandEnvironmentVariables("%ProgramW6432%") + @"\Unity\Editor\Data\Managed\UnityEngine.dll");
            // True - memory generation, false - external file generation
            _parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            _parameters.GenerateExecutable = false;
            // TODO : check path existence
            Directory.CreateDirectory("Assets/Plugins");
            _parameters.OutputAssembly = "Assets/Plugins/DulyGeneratedAssembly.dll";
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