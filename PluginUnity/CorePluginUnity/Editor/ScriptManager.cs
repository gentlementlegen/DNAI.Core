using Core.Plugin.Unity.Generator;
using CoreCommand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.Plugin.Editor
{
    /// <summary>
    /// Manages the loaded scripts for Duly.
    /// </summary>
    [Serializable]
    public class ScriptManager
    {
        /// The name of the file.
        public string FileName
        { get; set; }

        /// The path to the loaded file.
        public string FilePath
        { get; set; }

        /// The current processing status.
        public string ProcessingStatus
        { get; private set; }

        /// The list of IA references contained in the Duly file.
        public List<KeyValuePair<string, Type>> iaList = new List<KeyValuePair<string, Type>>();

        /// <summary>
        /// A reference to a file loader, able to manage loading of Duly files.
        /// </summary>
        private readonly IManager _manager = new ProtobufManager();

        private readonly DulyCodeConverter _codeConverter;

        public List<string> FunctionList { get; private set; } = new List<string>();

        public ScriptManager(string filePath = "", string fileName = "")
        {
            FileName = fileName;
            FilePath = filePath;
            ProcessingStatus = "No file selected.";
            //fileLoader.onFileLoaded += OnScriptLoaded;
            _codeConverter = new DulyCodeConverter(_manager as ProtobufManager);
        }

        /// <summary>
        /// Loads the script at a certain location.
        /// </summary>
        /// <param name="filePath">File path.</param>
        public void LoadScript(string filePath)
        {
            FilePath = filePath;
            LoadScript();
        }

        /// <summary>
        /// Loads the script using <paramref name="FilePath"/> path.
        /// </summary>
        public void LoadScript()
        {
            if (FilePath?.Length == 0)
            {
                ProcessingStatus = "No file selected.";
                return;
            }
            ProcessingStatus = "Reading file...";
            //Thread t = new Thread (() => fileLoader.LoadFile(FilePath));
            //t.Start ();
            var fileCopyPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(ScriptManager)).Location), "..", "DulyAssets");
            Directory.CreateDirectory(fileCopyPath);
            File.Copy(FilePath, Path.Combine(fileCopyPath, Path.GetFileName(FilePath)));

            Task.Run(() =>
            {
                _manager.LoadCommandsFrom(FilePath);
                //var codeConverter = new DulyCodeConverter(_manager as ProtobufManager);
                //codeConverter.ConvertCode();
            }).ContinueWith((param) => OnScriptLoaded(param.Status.ToString()));
        }

        /// <summary>
        /// Callback called when a script is loaded.
        /// </summary>
        /// <param name="status">Status.</param>
        private void OnScriptLoaded(string status)
        {
            //var scriptList = fileLoader.GetFileContent();

            ProcessingStatus = status;
            //iaList = scriptList.ConvertAll(
            //	x => new KeyValuePair<string, Type>(x.Key, Type.GetType(x.Value))
            //);
            FunctionList = _codeConverter.FetchFunctions();
        }

        /// <summary>
        /// Compile the loaded code to the library.
        /// </summary>
        /// <param name="functionIds"></param>
        public void Compile(IEnumerable<int> functionIds)
        {
            _codeConverter.ConvertCode(functionIds);
        }
    }
}