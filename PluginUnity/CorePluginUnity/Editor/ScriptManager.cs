using Core.Plugin.Unity.Generator;
using CoreCommand;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Core.Plugin.Unity.Editor
{
    /// <summary>
    /// Manages the loaded scripts for DNAI.
    /// </summary>
    [System.Serializable]
    public class ScriptManager
    {
        /// <summary>
        /// A reference to a file loader, able to manage loading of Duly files.
        /// </summary>
        private readonly BinaryManager _manager = new BinaryManager();

        private readonly DulyCodeConverter _codeConverter;

        public List<string> FunctionList { get; private set; } = new List<string>();

        /// The name of the file.
        [UnityEngine.SerializeField]
        private string _fileName;
        public string FileName
        { get => _fileName; set => _fileName = value; }

        /// The path to the loaded file.
        [UnityEngine.SerializeField]
        private string _filePath;
        public string FilePathRelative
        { get => _filePath; set => _filePath = value; }

        [UnityEngine.SerializeField]
        private string _filePathAbsolute;
        public string FilePathAbsolute
        { get => _filePathAbsolute; set => _filePathAbsolute = value; }

        /// The current processing status.
        private string _processingStatus;
        public string ProcessingStatus
        { get => _processingStatus; set => _processingStatus = value; }

        public string AssemblyName
        { get => _codeConverter.AssemblyName; }

        [UnityEngine.SerializeField]
        private string _scriptName;
        public string ScriptName => _scriptName;

        /// The list of IA references contained in the Duly file.
        public List<KeyValuePair<string, Type>> iaList = new List<KeyValuePair<string, Type>>();

        public ScriptManager()
        {
            ProcessingStatus = "No file selected.";
            _codeConverter = new DulyCodeConverter(_manager as BinaryManager);
        }

        //public ScriptManager(string filePath = "", string fileName = "")
        //{
        //    FileName = fileName;
        //    FilePath = filePath;
        //    ProcessingStatus = "No file selected.";
        //    //fileLoader.onFileLoaded += OnScriptLoaded;
        //    _codeConverter = new DulyCodeConverter(_manager as ProtobufManager);
        //}

        /// <summary>
        /// Loads the script at a certain location.
        /// </summary>
        /// <param name="filePath">File path.</param>
        //public void LoadScript(string filePath)
        //{
        //    FilePath = filePath;
        //    LoadScript();
        //}

        /// <summary>
        /// Loads the script using <paramref name="FilePath"/> path.
        /// </summary>
        public string LoadScript(string path)
        {
            if (path?.Length == 0)
            {
                ProcessingStatus = "No file selected.";
                return "";
            }
            ProcessingStatus = "Reading file...";
            //Thread t = new Thread (() => fileLoader.LoadFile(FilePath));
            //t.Start ();
            //UnityEngine.Debug.Log("file path => " + path);
            var fileCopyPath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetAssembly(typeof(ScriptManager)).Location), "..", "Scripts");
            var fileFullPath = Path.GetFullPath(Path.Combine(fileCopyPath, Path.GetFileName(path)));

            try
            {
                // TODO : maybe check if the file is already there and ask for overwrite
                CloudFileWatcher.Watch(false);
                Directory.CreateDirectory(fileCopyPath);
                File.Copy(path, fileFullPath, true);
                CloudFileWatcher.Watch(true);
            }
            catch (IOException e)
            {
                //UnityEngine.Debug.LogWarning($"Error copying file at location [{path}]: {e.Message}");
            }
            finally
            {
                CloudFileWatcher.Watch(true);
            }

            Task.Run(() =>
            {
                _manager.Reset();
                _manager.LoadCommandsFrom(path);
                _scriptName = GetScriptName();
                //var codeConverter = new DulyCodeConverter(_manager as ProtobufManager);
                //codeConverter.ConvertCode();
            }).ContinueWith((param) => OnScriptLoaded(param.Status.ToString()));
            FilePathAbsolute = fileFullPath;
            return Constants.ScriptPath + Path.GetFileName(path);
        }

        public void ReloadScript()
        {
            _manager.Reset();
            _manager.LoadCommandsFrom(FilePathAbsolute);
        }

        public string GetLoadedScriptName()
        {
            return _manager.Controller.GetMainContextName();
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
            // This case happens when Unity ddeserializes the object.
            // Since we don't want to slow down the ctor too much, it's better
            // to check it on Compile call
            if (_manager.FilePath == null && _filePath != null)
                _manager.LoadCommandsFrom(_filePath);
            _codeConverter.ConvertCode(functionIds);
        }

        /// <summary>
        /// Compile the loaded code to the library asynchronously.
        /// </summary>
        /// <param name="functionIds"></param>
        /// <returns></returns>
        public Task CompileAsync(IEnumerable<int> functionIds)
        {
            // This case happens when Unity ddeserializes the object.
            // Since we don't want to slow down the ctor too much, it's better
            // to check it on Compile call
            if (_manager.FilePath == null && _filePath != null)
                _manager.LoadCommandsFrom(_filePath);
            return Task.Run(() => _codeConverter.ConvertCode(functionIds));
        }

        /// <summary>
        /// Compile the loaded code to the library asynchronously.
        /// </summary>
        /// <returns></returns>
        public Task CompileAsync()
        {
            // This case happens when Unity deserializes the object.
            // Since we don't want to slow down the ctor too much, it's better
            // to check it on Compile call
            // TODO : Add interface for deserialization instead ? 
            if (_manager.FilePath == null && _filePath != null)
                _manager.LoadCommandsFrom(_filePath);
            return Task.Run(() => _codeConverter.ConvertCode());
        }

        /// <summary>
        /// Gets the main script class name.
        /// </summary>
        /// <returns></returns>
        public string GetScriptName()
        {
            if (_manager?.Controller != null)
            {
                return _manager.Controller.GetMainContextName() ?? "Script";
            }
            return "Script";
        }
    }
}