//#define UNITY_ENGINE

using Core.Plugin.Unity.Editor;
using Core.Plugin.Unity.Extensions;
using CoreCommand;
using Microsoft.CSharp;
using Microsoft.Win32;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using static CoreControl.EntityFactory;
using System.Diagnostics;
using System.Runtime.InteropServices;

[assembly: InternalsVisibleTo("TestUnityPlugin")]

public static class OperatingSystem
{
    public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsMacOS() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
}

namespace Core.Plugin.Unity.Generator
{
    /// <summary>
    /// Manages code conversion from DNAI files to Unity files.
    /// </summary>
    public class DulyCodeConverter
    {
        public string AssemblyName { get; private set; }

        private readonly Compiler _compiler = new Compiler();
        private readonly TemplateReader _template = new TemplateReader();
        private readonly BinaryManager _manager;

        public DulyCodeConverter(BinaryManager manager)
        {
            _manager = manager;
        }

        /// <summary>
        /// Transform Duly code to Unity code.
        /// </summary>
        public void ConvertCode()
        {
            var variables = new List<Entity>();
            var functions = new List<Entity>();
            var dataTypes = new List<Entity>();

            AssemblyName = Path.GetFileNameWithoutExtension(_manager.FilePath).RemoveIllegalCharacters();

            var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);

            foreach (var id in ids)
            {
                dataTypes.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.DATA_TYPE, id));
                variables.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, id));
                functions.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, id));
            }

            var code = _template.GenerateTemplateContent(_manager, variables, functions, dataTypes);
            _compiler.Compile(code, AssemblyName);
        }

        /// <summary>
        /// Transforms Duly code to Unity code only keeping the specified functions.
        /// </summary>
        /// <param name="functionIds">The ids of the functions to keep.</param>
        public void ConvertCode(IEnumerable<int> functionIds)
        {
            if (_manager.FilePath == null)
                throw new NullReferenceException("Path is null");
            AssemblyName = Path.GetFileNameWithoutExtension(_manager.FilePath).RemoveIllegalCharacters();

            var variables = new List<Entity>();
            var functions = new List<Entity>();
            var dataTypes = new List<Entity>();

            var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);
            //var variables = _manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, ids[0]);
            //var functions = _manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, ids[0]);
            foreach (var id in ids)
            {
                dataTypes.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.DATA_TYPE, id));
                variables.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.VARIABLE, id));
                functions.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, id));
            }
            var funcToKeep = new List<Entity>();
            foreach (var id in functionIds)
            {
                funcToKeep.Add(functions[id]);
            }
            var code = _template.GenerateTemplateContent(_manager, variables, funcToKeep, dataTypes);
            _compiler.Compile(code, AssemblyName);
        }

        /// <summary>
        /// Retrieves all the functions available in the AI file, as names.
        /// </summary>
        /// <returns></returns>
        public List<string> FetchFunctions()
        {
            var ids = _manager.Controller.GetIds(EntityType.CONTEXT | EntityType.PUBLIC);
            var funcs = new List<Entity>();
            foreach (var id in ids)
                funcs.AddRange(_manager.Controller.GetEntitiesOfType(ENTITY.FUNCTION, id));
            return funcs.ConvertAll(x => x.Name.SplitCamelCase());
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

        private const string AssemblyOutputPath = Constants.CompiledPath;
#if UNITY_ENGINE
        private const string assemblyPath = Constants.PluginsPath;
#else
        private const string assemblyPath = "";
#endif

        //internal CompilerResults Compile(string code, string outputPath = "./")
        //{
        //    this.code = code;
        //    return Compile();
        //}

        internal Compiler()
        {
            Directory.CreateDirectory(Constants.PluginsPath);
            Directory.CreateDirectory(AssemblyOutputPath);
            //_parameters.OutputAssembly = "Assets/Plugins/" + assemblyName + ".dll";
            // Reference to library
            string unityLibPath = GetUnityLibraryPath();
            if (string.IsNullOrEmpty(unityLibPath))
                throw new DllNotFoundException("Unity library could not be found.");

            //    _parameters.ReferencedAssemblies.Add(unityLibPath + @"\Editor\Data\Managed\UnityEngine.dll");
            //_parameters.ReferencedAssemblies.Add(unityLibPath + @"UnityEngine.dll");


            //_parameters.ReferencedAssemblies.Add(Environment.ExpandEnvironmentVariables("%ProgramW6432%") + @"\Unity\Editor\Data\Managed\UnityEngine.dll");
          //  _parameters.ReferencedAssemblies.Add(assemblyPath + "CoreCommand.dll");
          //  _parameters.ReferencedAssemblies.Add(assemblyPath + "CoreControl.dll");


            if (OperatingSystem.IsMacOS())
            {
                _parameters.ReferencedAssemblies.Add(unityLibPath + @"UnityEngine.dll"); //UnityEngine.CoreModule.dll
                _parameters.ReferencedAssemblies.Add(unityLibPath + @"UnityEngine.CoreModule.dll");

                _parameters.ReferencedAssemblies.Add(assemblyPath + "CoreCommand.dll");
                _parameters.ReferencedAssemblies.Add(assemblyPath + "CoreControl.dll");

                _parameters.ReferencedAssemblies.Add("/Library/Frameworks/Mono.framework/Versions/5.4.0/lib/mono/4.6.1-api/System.Core.dll");
                _parameters.ReferencedAssemblies.Add("/Library/Frameworks/Mono.framework/Versions/5.4.0/lib/mono/4.6.1-api/Microsoft.CSharp.dll");
            } else if (OperatingSystem.IsWindows()) {
                _parameters.ReferencedAssemblies.Add(unityLibPath + @"\Editor\Data\Managed\UnityEngine.dll");

                _parameters.ReferencedAssemblies.Add(assemblyPath + "CoreCommand.dll");
                _parameters.ReferencedAssemblies.Add(assemblyPath + "CoreControl.dll");

                _parameters.ReferencedAssemblies.Add(assemblyPath + "../Editor/CorePluginUnity.dll");
                _parameters.ReferencedAssemblies.Add(unityLibPath + @"\Editor\Data\Managed\UnityEditor.dll");

                _parameters.ReferencedAssemblies.Add("System.Core.dll");
                _parameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
            }
            else
            {
                throw new DllNotFoundException("Not supported on linux.");
            }

            // True - memory generation, false - external file generation
            _parameters.GenerateInMemory = true;
            // True - exe file generation, false - dll file generation
            _parameters.GenerateExecutable = false;
            //_parameters.CompilerOptions += $"-doc:{assemblyPath}{assemblyName}.xml";
        }

        private string GetUnityLibraryPath()
        {
            var path = "";

            if (OperatingSystem.IsWindows())
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Unity Technologies\Installer\Unity"))
                {
                    Debug.Print(key.ToString());
                    if (key != null)
                    {
                        Object o = key.GetValue("Location x64");
                        if (o != null)
                        {
                            path = o as string;
                        }
                    }
                }
            }
            else if (OperatingSystem.IsMacOS())
            {
                path = "/Applications/Unity/Contents/Frameworks/";
            }

            return path;
        }

        /// <summary>
        /// Compiles the code to an assembly. <para/>
        /// Throws <see cref="InvalidOperationException"/> on compilation failure.
        /// </summary>
        /// <param name="code">The code to compile.</param>
        /// <param name="assemblyName">The name of the assembly output.</param>
        internal CompilerResults Compile(string code, string assemblyName = "DulyGeneratedAssembly")
        {
            _parameters.OutputAssembly = AssemblyOutputPath + assemblyName + ".dll";
            _parameters.CompilerOptions = $"-doc:\"{AssemblyOutputPath}{assemblyName}.xml\"";

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