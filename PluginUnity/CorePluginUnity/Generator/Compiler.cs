#define UNITY_ENGINE
using CoreCommand;
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using static CoreControl.EntityFactory;

[assembly: InternalsVisibleTo("TestUnityPlugin")]

namespace Core.Plugin.Unity.Generator
{
    /// <summary>
    /// Manages code conversion from Duly files to Unity files.
    /// </summary>
    public class DulyCodeConverter
    {
        private readonly Compiler _compiler = new Compiler();
        private readonly TemplateReader _template = new TemplateReader();
        private readonly ProtobufManager _manager;

        public DulyCodeConverter(ProtobufManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Transform Duly code to Unity code.
        /// </summary>
        public void ConvertCode()
        {
            var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);
            var variables = _manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, ids[0]);
            var functions = _manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, ids[0]);
            var code = _template.GenerateTemplateContent(_manager, variables, functions);
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
#if UNITY_ENGINE
            const string assemblyPath = "Assets/Plugins/";
#else
            const string assemblyPath = "";
#endif
            Directory.CreateDirectory("Assets/Plugins");
            _parameters.OutputAssembly = "Assets/Plugins/DulyGeneratedAssembly.dll";
            // Reference to library
            // TODO : change with instllation program
            _parameters.ReferencedAssemblies.Add(Environment.ExpandEnvironmentVariables("%ProgramW6432%") + @"\Unity\Editor\Data\Managed\UnityEngine.dll");
            _parameters.ReferencedAssemblies.Add(assemblyPath + "CoreCommand.dll");
            _parameters.ReferencedAssemblies.Add(assemblyPath + "CoreControl.dll");
            _parameters.ReferencedAssemblies.Add("System.Core.dll");
            // True - memory generation, false - external file generation
            _parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            _parameters.GenerateExecutable = false;
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